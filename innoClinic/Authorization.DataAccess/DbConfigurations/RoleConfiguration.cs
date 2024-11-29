using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.DataAccess.DbConfigurations {
    internal sealed class RoleConfiguration: IEntityTypeConfiguration<Role> {
        private const string TABLE_NAME = "Roles";
        public void Configure( EntityTypeBuilder<Role> builder ) {
            builder.ToTable( TABLE_NAME );
            builder.HasKey( role => role.Id );
            builder.Property( role => role.Id ).ValueGeneratedNever();
            builder.Property( role => role.Name ).HasMaxLength( 140 ).IsRequired( required: true );
        }
    }
}
