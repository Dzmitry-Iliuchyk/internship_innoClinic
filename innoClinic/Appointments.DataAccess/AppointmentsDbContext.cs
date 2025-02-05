using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.DataAccess {
    public class AppointmentsDbContext: DbContext {
        public DbSet<Result> Results { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public AppointmentsDbContext( DbContextOptions options ) : base( options ) {
        }
        protected override void OnModelCreating( ModelBuilder builder ) {
            builder.ApplyConfigurationsFromAssembly( typeof( AppointmentConfiguration ).Assembly );
        }
    }
    public class AppointmentConfiguration: IEntityTypeConfiguration<Appointment> {
        public void Configure( EntityTypeBuilder<Appointment> builder ) {
            builder.HasKey( a => a.Id );

            builder.Property( a => a.DoctorId )
                .IsRequired();

            builder.Property( a => a.ServiceId )
                .IsRequired();

            builder.Property( a => a.OfficeId )
                .IsRequired();

            builder.Property( a => a.PatientId )
                .IsRequired();

            builder.Property( a => a.StartTime )
                .IsRequired();

            builder.Property( a => a.Duration )
                .IsRequired();

        }
    }
    public class PatientConfiguration: IEntityTypeConfiguration<Patient> {
        public void Configure( EntityTypeBuilder<Patient> builder ) {
            builder.HasKey( a => a.Id );
            builder.HasMany<Appointment>().WithOne( x => x.Patient ).OnDelete( DeleteBehavior.Restrict );
            builder.Property( a => a.PatientFirstName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.PatientSecondName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.PatientEmail )
                .IsRequired()
                .HasMaxLength( 80 );
        }
    }
    public class OfficeConfiguration: IEntityTypeConfiguration<Office> {
        public void Configure( EntityTypeBuilder<Office> builder ) {
            builder.HasKey( a => a.Id );

            builder.HasMany<Appointment>().WithOne(x=>x.Office).OnDelete( DeleteBehavior.Restrict );
            
            builder.Property( a => a.Id )
                .HasMaxLength( 30 );
            
            builder.Property( a => a.OfficeAddress )
                .IsRequired()
                .HasMaxLength( 300 );
        }
    }
    public class ServiceConfiguration: IEntityTypeConfiguration<Service> {
        public void Configure( EntityTypeBuilder<Service> builder ) {
            builder.HasKey( a => a.Id  );

            builder.HasMany<Appointment>().WithOne( x => x.Service ).OnDelete( DeleteBehavior.Restrict );
            
            builder.Property( a => a.Id )
                .ValueGeneratedNever();

            builder.Property( a => a.ServiceName )
                .IsRequired()
                .HasMaxLength( 120 );

            builder.Property( a => a.ServicePrice )
                .HasPrecision(2)
                .IsRequired();
        }
    }
    public class DoctorConfiguration: IEntityTypeConfiguration<Doctor> {
        public void Configure( EntityTypeBuilder<Doctor> builder ) {
            builder.HasKey( a => a.Id );

            builder.HasMany<Appointment>().WithOne( x => x.Doctor ).OnDelete(DeleteBehavior.Restrict);

            builder.Property( a => a.DoctorFirstName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.DoctorSecondName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.DoctorSpecialization )
                .IsRequired()
                .HasMaxLength( 120 );
        }
    }
    public class ResultConfiguration: IEntityTypeConfiguration<Result> {
        public void Configure( EntityTypeBuilder<Result> builder ) {
            builder.HasKey( a => a.Id );

            builder.Property( a => a.Conclusion )
                .HasMaxLength( 3000 )
                .IsRequired();

            builder.Property( a => a.DocumentUrl )
                .IsRequired()
                .HasMaxLength( 300 );

            builder.Property( a => a.Complaints )
                .IsRequired()
                .HasMaxLength( 500 );

            builder.Property( a => a.Recomendations )
                .IsRequired()
                .HasMaxLength( 1000 );

        }
    }

}
