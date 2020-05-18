using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Snail.Permission.Test
{
    /// <summary>
    /// 参考：https://docs.microsoft.acom/zh-cn/ef/core/miscellaneous/cli/dbcontext-creation
    /// 作用：用于配置Migration，当用Add-Migration命令时，会基于此类生成dbContext去对数据库进行操作
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseMySql(@"Server =localhost; Port =3306; Database =sample; User Id = root; Password = root;");
            //optionsBuilder.UseSqlServer(@"Server =localhost\sqlexpress; Database =sample; User Id = sa; Password = test;");
            return new TestDbContext(optionsBuilder.Options);
        }
    }
}
