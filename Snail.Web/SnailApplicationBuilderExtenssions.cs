using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSwag;
using Snail.Core;
using Snail.Core.Dto;
using Snail.Web.Controllers;
using Snail.Web.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Snail.Web
{
    public static class SnailApplicationBuilderExtenssions
    {
        public static void ConfigSnailWebApplicationBuilder(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider,IConfiguration configuration)
        {
            // 获取autofac容器
            var testName = configuration.GetValue<string>("test:name");
            Console.WriteLine($"--------------test:name:-------------{testName}");
            if (env.IsDevelopment())
            {
                app.UseMiniProfiler();
                app.UseDeveloperExceptionPage(); //开发环境用异常处理程序页，让开发者能看到异常信息详细
            }
            else
            {
                // 生产环境异常处理，隐藏异常详细信息，并记录日志
                app.UseExceptionHandler(errorApp =>
                {

                    errorApp.Run(async context =>
                    {
                        var loggerFactory = (ILoggerFactory)context.RequestServices.GetService(typeof(ILoggerFactory));
                        var logger = loggerFactory.CreateLogger("UnKnowException");
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        //业务异常
                        ApiResultDto responseResultModel;
                        if (exceptionHandlerPathFeature?.Error is BusinessException businessException)
                        {
                            responseResultModel = ApiResultDto.BadRequestResult(businessException.Message);
                            if (logger != null)
                            {
                                logger.LogError(exceptionHandlerPathFeature?.Error?.ToString());
                            }
                        }
                        else
                        {
                            responseResultModel = ApiResultDto.BadRequestResult($"程序出错，出于安全考虑，出错信息未能返回，请联系IT进行处理，错误时间{DateTime.Now}");
                            if (logger != null)
                            {
                                logger.LogError(exceptionHandlerPathFeature?.Error?.ToString());
                            }
                        }
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(responseResultModel));
                    });
                });


                //HTTP严格传输安全 让网站可以通知浏览器它不应该再使用HTTP加载该网站，而是自动转换该网站的所有的HTTP链接至更安全的HTTPS。它包含在HTTP的协议头 Strict-Transport-Security 中，在服务器返回资源时带上,换句话说，它告诉浏览器将URL协议从HTTP更改为HTTPS（会更安全），并要求浏览器对每个请求执行此操作。
                //正式环境官方建议用UseHsts和UseHttpsRedirection，
                // 如果反方代理服务器，如ngix已经有配置过http重定向https或是设置hsts，则不需要设置这两句
                //参考: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio
                app.UseHsts();
                app.UseHttpsRedirection();//将所有的http重定向https
            }



            //静态文件
            //app.UseStaticFiles();
            ////spa前端静态文件
            //app.UseSpaStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    RequestPath = "/" + serviceProvider.GetService<IOptions<StaticFileUploadOption>>().Value.StaticFilePath,
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, serviceProvider.GetService<IOptions<StaticFileUploadOption>>().Value.StaticFilePath))
            //});

            app.UseCors(builder => { builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); });

            //app.UseApplicationLicensing();
            app.UseAuthentication();

            // hangfire前端界面的访问控制
            app.UseHangfireDashboard(options: new Hangfire.DashboardOptions
            {
                //Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });

            #region 3.1模板 的mvc
            app.UseRouting();
            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapHub<DefaultHub>("/defaultHub");
                endpoints.MapControllers();
            });

            #endregion

            #region swagger
            if (configuration.GetValue<bool>("EnableSwagger"))
            {
                //* 如果出现如下错误：Fetch errorundefined / swagger / v1 / swagger.json
                //* 解决：原因是swagger 的api在解析时出错，在chrome f12看具体请求swagger.json的错误，解决
                // UseOpenApi用于生成swagger/v1/swagger.json文档，此文档是UseSwaggerUi3和UseReDoc界面生成的前提条件
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
                //app.UseReDoc();//UseReDoc和UseSwaggerUi3任意用一个即可，UseSwaggerUi3生成的Ui界面可调用接口，而UseReDoc生成只读的接口文档
            }

            #endregion


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp/dist";
                //下面是vs模板对spa应用的默认配置，推荐关闭，改用 webpack-dev-server + api proxy 来提高开发速度
                //if (env.IsDevelopment())
                //{
                //    spa.UseReactDevelopmentServer(npmScript: "start");
                //}
            });


        }
    }
}
