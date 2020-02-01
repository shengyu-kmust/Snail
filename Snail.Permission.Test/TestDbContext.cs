using Microsoft.EntityFrameworkCore;
using Snail.Permission.Entity;

namespace Snail.Permission.Test
{
    public class TestDbContext : PermissionDatabaseContext
    {
        public TestDbContext()
        {

        }
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            else
            {
                optionsBuilder.UseSqlServer(@"Server =localhost\sqlexpress; Database =sample; User Id = sa; Password = test;");
            }

        }
    }
}
