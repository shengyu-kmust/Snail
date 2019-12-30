using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<TestDbContext>();
            services.AddPermission<TestDbContext>(options =>
            {
                options.RsaPrivateKey = Configuration.GetSection("PermissionOptions:RsaPrivateKey").Value;
                options.RsaPublicKey = Configuration.GetSection("PermissionOptions:RsaPublicKey").Value;
                options.PasswordSalt = Configuration.GetSection("PermissionOptions:PasswordSalt").Value;
                options.ResourceAssemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
            });
            //services.AddDbContext<TestDbContext>(options =>
            //{
            //    options.UseMySql("Server=localhost;Port=3306;Database=permissionTest;User Id=root;Password = root;");
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
