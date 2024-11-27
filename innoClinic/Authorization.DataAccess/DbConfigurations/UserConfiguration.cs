using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.DataAccess.DbConfigurations {
    internal sealed class UserConfiguration: IEntityTypeConfiguration<User> {
        private const string TABLE_NAME = "Users";
        public void Configure( EntityTypeBuilder<User> builder ) {
            builder.ToTable( TABLE_NAME );
            builder.HasKey( user => user.Id );
            builder.Property( user => user.Email ).HasMaxLength( 140 ).IsRequired( required: true );
            builder.Property( user => user.PasswordHash ).IsRequired( required: true );
            builder.HasMany( user => user.Roles )
                .WithMany()
                .UsingEntity<UserRole>(
                     l => l.HasOne( e => e.Role ).WithMany().HasForeignKey( e => e.RoleId ),
                     r => r.HasOne( e => e.User ).WithMany().HasForeignKey( e => e.UserId ),
                     j => {
                         j.HasKey( x => new { x.RoleId, x.UserId } );
                         j.Property( e => e.CreatedAt ).HasDefaultValueSql( "CURRENT_TIMESTAMP" );
                     } );

        }
    }
}
