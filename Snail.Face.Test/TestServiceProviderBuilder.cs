using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Snail.Face.Test
{
    public class TestServiceProviderBuilder
    {
        [Fact]
        public static ServiceProvider Build()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DbContext,SnailFaceTestDb>(option =>
            {
                option.UseSqlite("Data Source=E:\\faceapp.db;");
            });
            services.AddTransient<ISnailFaceRecognition, SnailFaceRecognition>();
            services.AddOptions<SnailFaceRecognitionOption>().Configure(opt =>
            {
                opt.ModelsDirectory = @"G:\mywork\Snail\Snail\Snail.Face.Test\models";
                opt.Tolerance = 0.6d;
            });
            return services.BuildServiceProvider();
        }

    }
}
