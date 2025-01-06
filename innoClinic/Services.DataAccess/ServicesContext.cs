using Microsoft.EntityFrameworkCore;
using Services.Domain;


namespace Services.DataAccess {

    public class ServiceContext: DbContext {
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public ServiceContext( DbContextOptions options ) : base( options ) {
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<Service>( entity => {
                entity.ToTable( "Services" );
                entity.HasKey( e => e.Id );
                entity.Property( e => e.Id ).ValueGeneratedNever();
                entity.Property( e => e.Name ).IsRequired().HasMaxLength( 80 );
                entity.HasIndex( e => e.Name ).IsUnique( true );
                entity.Property( e => e.Price ).IsRequired();
                entity.Property( e => e.CategoryId ).IsRequired();
                entity.Property( e => e.SpecializationId ).IsRequired();
                entity.Property( e => e.IsActive ).IsRequired();

                entity.HasOne( e => e.Category )
                      .WithMany()
                      .HasForeignKey( e => e.CategoryId );

                entity.HasOne( e => e.Specialization )
                      .WithMany()
                      .HasForeignKey( e => e.SpecializationId );
            } );
            modelBuilder.Entity<ServiceCategory>( entity => {
                entity.ToTable( "ServiceCategories" );
                entity.HasKey( e => e.Id );
                entity.Property( e => e.Id ).ValueGeneratedOnAdd();
                entity.HasIndex( e => e.Name ).IsUnique(true);
                entity.Property( e => e.Name ).IsRequired().HasMaxLength( 80 );
                entity.Property( e => e.TimeSlotSize ).IsRequired();
            } );

            modelBuilder.Entity<Specialization>( entity => {
                entity.ToTable( "Specializations" );
                entity.HasKey( e => e.Id );
                entity.Property( e => e.Id ).ValueGeneratedOnAdd();
                entity.HasIndex( e => e.Name ).IsUnique(true);
                entity.Property( e => e.Name ).IsRequired().HasMaxLength( 80 );
                entity.Property( e => e.IsActive ).IsRequired();
            } );
        }
    }
}