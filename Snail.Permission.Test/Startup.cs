using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSwag;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Snail.Permission.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseMySql("Server=localhost;Port=3306;Database=permissionTest;User Id=root;Password = root;");
            });
            #region 默认权限数据结构
            //services.AddDefaultPermission<TestDbContext>(options =>
            //{
            //    Configuration.GetSection("PermissionOptions").Bind(options);
            //    options.ResourceAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
            //});
            #endregion

            #region 自定义权限数据结构
            services.AddPermission<TestDbContext, User>(options =>
            {
                Configuration.GetSection("PermissionOptions").Bind(options);
                options.ResourceAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
            });
            services.TryAddScoped<IPermissionStore, CustomPermissionStore>();
            #endregion


            
            services.AddControllers();
            #region swagger
            services.AddOpenApiDocument(conf => {
                conf.Description = "change the description";
                conf.DocumentName = "change the document name";
                conf.GenerateExamples = true;
                conf.Title = "change the title";
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


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(config =>
            {
                config.MapControllers();
            });
            

            #region swag
            //* 如果出现如下错误：Fetch errorundefined / swagger / v1 / swagger.json
            //* 解决：原因是swagger 的api在解析时出错，在chrome f12看具体请求swagger.json的错误，解决
            app.UseOpenApi(config =>
            {
                config.PostProcess = (document, req) =>
                {
                    //下面是向swag怎加https和http的两种方式
                    document.Schemes.Add(OpenApiSchema.Https);
                    document.Schemes.Add(OpenApiSchema.Http);
                };
            });
            app.UseSwaggerUi3();
            //app.UseReDoc();
            #endregion
            serviceProvider.GetService<TestDbContext>().Database.Migrate();//自动migrate，前提是程序集里有add-migrate后的类
        }
    }
}
