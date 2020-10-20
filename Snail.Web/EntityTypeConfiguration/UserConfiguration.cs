using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snail.Permission.Entity;

namespace Snail.Web.EntityTypeConfiguration
{
    public class UserConfiguration : BaseConfiguration,IEntityTypeConfiguration<PermissionDefaultUser >
    {
        public void Configure(EntityTypeBuilder<PermissionDefaultUser > builder)
        {
            base.Config(builder);
            builder.ToTable("User");
            builder.Property(a => a.Gender).HasConversion<string>();
        }
    }
}
