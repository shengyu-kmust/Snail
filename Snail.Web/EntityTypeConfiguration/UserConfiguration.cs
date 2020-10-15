using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snail.Permission.Entity;

namespace Snail.Web.EntityTypeConfiguration
{
    public class UserConfiguration : BaseConfiguration,IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            base.Config(builder);
            builder.ToTable("User");
            builder.Property(a => a.Gender).HasConversion<string>();
        }
    }
}
