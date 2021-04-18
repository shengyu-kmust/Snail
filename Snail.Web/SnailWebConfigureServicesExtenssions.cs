using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.MySql;
using Hangfire.SQLite;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using NSwag;
using Savorboard.CAP.InMemoryMessageQueue;
using Snail.Cache;
using Snail.Core;
using Snail.Core.Default;
using Snail.Core.Interface;
using Snail.Core.Permission;
using Snail.FileStore;
using Snail.Office;
using Snail.Permission;
using Snail.Web.Filter;
using System;
using System.Linq;
using IFileProvider = Snail.FileStore.IFileProvider;


namespace Snail.Web
{
    public static class SnailWebConfigureServicesExtenssions
    {
        /// <summary>
        /// 注册snailWeb框架的service依赖,如：
        /// MVC、cap、healthCheck、signalr、spa、swagger、easycaching、snailCache、hangfire、miniProfiler、option配置、日志、缓存、应用上下文、实体缓存、license、excel、文件存储
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigSnailWebServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)

        {
            #region option配置
            services.ConfigAllOption(configuration);
            #endregion

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddMemoryCache();
            services.TryAddScoped<IApplicationContext, ApplicationContext>();
            services.TryAddScoped<IEntityCacheManager, EntityCacheManager>();
            services.AddScoped<ICapSubscribe, EntityCacheManager>();//将EntityCacheManager注册为ICapSubscribe,使SnailCapConsumerServiceSelector能注册监听方法
            services.AddHttpContextAccessor();//注册IHttpContextAccessor，在任何地方可以通过此对象获取httpcontext，从而获取单前用户
            services.AddApplicationLicensing(configuration.GetSection("ApplicationlicensingOption")); // 注册程序license
            services.AddResponseCaching();
            services.AddTransient<IExcelHelper, ExcelNPOIHelper>();

            #region 增加文件存储
            services.AddScoped<IFileStore, EFFileStore>();

            // todo 可以抽离到外部进行配置
            // 本地存储
            services.AddScoped<IFileProvider, DictoryFileProvider>();
            services.AddOptions<DictoryFileProviderOption>().Configure(option => { option.BasePath = "./staticFile"; option.MaxLength = 60000; });


            // 数据库存储
            //services.AddScoped<IFileProvider, DatabaseFileProvider>();
            //services.AddScoped<IFileStore, EFFileStore>();


            // mongodb存储
            //services.AddScoped<IFileProvider, MongodbFileProvider>();
            //services.AddScoped<IFileStore, EFFileStore>();
            //services.AddOptions<MongodbFileProviderOption>().Configure(option => { option.ConnectString = "mongodb://localhost/admin"; option.DatabaseName = "admin"; });

            #endregion

        

            #region MVC
            //3.1模板的mvc
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilterAttribute>();
                options.Filters.Add<GlobalResultFilterAttribute>();
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                //.net core 3.0后，json用了system.text.json，但功能比较单一，可以换成用newtonsoftJson,参考  https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.0
                options.SerializerSettings.Converters.Add(new StringEnumConverter());//配置mvc的action返回格式化对enum类型的处理方式，将enum转成string返回
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    // 配置模型参数校验的返回结果，默认只要contoller上加上ApiController后，会以默认的400状态和错误结构返回，这里进行处理，统一返回BusinessException，并在ExceptionFilter这一层进行拦截处理
                    // 参考:https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#automatic-http-400-responses
                    var errorContext = string.Join(",", actionContext.ModelState.Values
                        .SelectMany(a => a.Errors.Select(b => b.ErrorMessage))
                        .ToArray());
                    throw new BusinessException($"参数验证失败：{errorContext}");
                };
            });

            #endregion

            #region signalr
            services.AddSignalR();
            #endregion

            #region 前端界面配置
            // In production, the front end files will be served from this directory
            // 在startup里设置，services.AddSpaStaticFiles和app.UseSpaStaticFiles是一对
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";//RootPath为相对路径，它的绝对路径为IWebHostEnvironment.ContentRootPath，配合app.UseSpaStaticFiles()使用。
            //});
            #endregion


            #region swagger
            services.AddOpenApiDocument(conf =>
            {
                conf.Description = "后台接口文档des";
                conf.DocumentName = "doc";
                conf.GenerateExamples = true;
                conf.Title = "后台接口文档title";
                conf.PostProcess = document =>
                {
                    document.SecurityDefinitions.Add(
                          "Jwt认证",
                          new OpenApiSecurityScheme
                          {
                              Type = OpenApiSecuritySchemeType.Http,
                              Name = "Authorization",//token会放到header的authorization里
                              In = OpenApiSecurityApiKeyLocation.Header,
                              Description = "请输入 : JWT token",
                              Scheme = "bearer"//定义bearer，不能改
                          });
                    document.Security.Add(new OpenApiSecurityRequirement { { "Jwt认证", new string[0] } });

                };
            }); // add OpenAPI v3 document
            #endregion

            #region 注入easyCaching
            // todo:是用easyCaching还是snailCache
            services.AddEasyCaching(option =>
            {
                //配置方式一：用config配置
                option.UseInMemory(configuration, "default", "easycaching:inmemory");

                //配置方式二：用代码的方式配置
                //option.UseInMemory(config =>
                //{
                //    config.DBConfig = new InMemoryCachingOptions
                //    {
                //        // scan time, default value is 60s
                //        ExpirationScanFrequency = 60,
                //        // total count of cache items, default value is 10000
                //        SizeLimit = 100
                //    };
                //    // the max random second will be added to cache's expiration, default value is 120
                //    config.MaxRdSecond = 0;
                //    // whether enable logging, default is false
                //    config.EnableLogging = false;
                //    // mutex key's alive time(ms), default is 5000
                //    config.LockMs = 5000;
                //    // when mutex key alive, it will sleep some time, default is 300
                //    config.SleepMs = 300;
                //}, "default");
            });
            #endregion

            #region 注入snailCache
            services.AddSnailMemoryCache();
            #endregion



            #region 定时任务
            var dbType = configuration.GetSection("DbSetting")["DbType"];
            var hangfireConnectString = configuration.GetSection("DbSetting")["Hangfire"];
            services.AddHangfireServer();//hangfire.aspnetcore的扩展，用这个方法后，hangfire的JobActivator会用asp.net core的IServiceProvider去做ioc，所以不需要额外配置JobActivator。而本项目Autofac已经实现了IServiceProvider
            services.AddHangfire(configuration =>
            {
                configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

                if (string.IsNullOrEmpty(hangfireConnectString))
                {
                    configuration.UseMemoryStorage();
                    return;
                }

                if (dbType.Equals("MySql", StringComparison.OrdinalIgnoreCase))
                {
                    configuration.UseStorage(new MySqlStorage(hangfireConnectString, new MySqlStorageOptions()
                    {
                        TablesPrefix = "hangfire_"
                    }));
                }
                else if (dbType.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                {
                    configuration.UseSqlServerStorage(hangfireConnectString, new SqlServerStorageOptions
                    {
                        //也可以换成mysql
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true
                    });
                }
                else if (dbType.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    configuration.UseSQLiteStorage(hangfireConnectString, new SQLiteStorageOptions
                    {
                        SchemaName = "hangfire"
                    });
                }
            });
            #endregion

           
            #region profiler
            //开发环境时，打开分析工具
            if (environment.IsDevelopment())
            {
                //访问示例：http://localhost:5000/profiler/results
                //profiler/results-index为列表
                // 参考：https://miniprofiler.com/dotnet/HowTo/ProfileEFCore
                services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();
            }
            #endregion

            #region 增加cap
            services.TryAddSingleton<IConsumerServiceSelector, SnailCapConsumerServiceSelector>();//默认的ConsumerServiceSelector实现不支持和autofac的完美结合，默认的实现的用法，是需用microsoft di进行服务注册后再调用service.AddCap。但用autofac后，所有的服务注册是在autofac里，即在下面的ConfigureContainer里，为了让cap知道事件和事件的处理方法，重写IConsumerServiceSelector的实现，SnailCapConsumerServiceSelector
            services.AddCap(option =>
            {
                option.UseInMemoryStorage();//用内存消息存储和队列 
                option.UseInMemoryMessageQueue();
                option.UseDashboard();//启用dashboard，默认路径为xxx/cap
            });
            #endregion

            #region health check
            //可以用：https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
            services.AddHealthChecks();
            #endregion

            services.AddCors();

            return services;
        }

        /// <summary>
        /// 注册snailWeb框架的权限和数据库配置。
        /// 数据库配置在json文件的DbSetting节点，权限option配置依赖于json文件的PermissionOptions节点
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <typeparam name="TUserRole"></typeparam>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TRoleResource"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigSnailWebDbAndPermission<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource,TKey>(this IServiceCollection services,IConfiguration configuration)
                    where TDbContext : DbContext
        where TUser : class, IUser, IIdField<TKey>, new()
        where TRole : class, IRole, IIdField<TKey>, new()
        where TUserRole : class, IUserRole, IIdField<TKey>, new()
        where TResource : class, IResource, IIdField<TKey>, new()
        where TRoleResource : class, IRoleResource, IIdField<TKey>, new()
        {
            #region 数据库配置
            var dbType = configuration.GetSection("DbSetting")["DbType"];
            var connectString = configuration.GetSection("DbSetting")["ConnectionString"];

            Action<DbContextOptionsBuilder> optionsAction = options =>
            {
                if (dbType.Equals("MySql", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseMySql(connectString);
                }
                else if (dbType.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlServer(connectString);
                }
                else if (dbType.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlite(connectString);
                }
            };
            services.AddDbContext<TDbContext>(optionsAction);
            services.AddDbContext<DbContext, TDbContext>(optionsAction);
            #endregion

            #region 增加通用权限
            services.AddPermission<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource,TKey>(options =>
            {
                configuration.GetSection("PermissionOptions").Bind(options);
                options.ResourceAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();// 从哪些程序集里，将Controller的action设置成权限资源
            });
            #endregion
            return services;
        }

    }
}
