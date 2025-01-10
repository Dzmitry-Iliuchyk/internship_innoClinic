using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace Appointments.DataAccess {
    public class AppointmentsDbContext: DbContext {
        public DbSet<Result> Results { get; set; }  
        public DbSet<Appointment> Appointments { get; set; }

        public AppointmentsDbContext( DbContextOptions options ) : base( options ) {
        }

        protected override void OnModelCreating( ModelBuilder builder ) {
            builder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
        }
    }

public class AppointmentConfiguration: IEntityTypeConfiguration<Appointment> {
        public void Configure( EntityTypeBuilder<Appointment> builder ) {
            builder.HasKey( a => a.Id );

            builder.Property( a => a.DoctorId )
                .IsRequired();

            builder.Property( a => a.DoctorFirstName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.DoctorSecondName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.DoctorSpecialization )
                .IsRequired()
                .HasMaxLength( 120 );

            builder.Property( a => a.ServiceId )
                .IsRequired();

            builder.Property( a => a.ServiceName )
                .IsRequired()
                .HasMaxLength( 120 );

            builder.Property( a => a.ServicePrice )
                .IsRequired();

            builder.Property( a => a.OfficeId )
                .IsRequired();

            builder.Property( a => a.OfficeAddress )
                .IsRequired()
                .HasMaxLength( 200 );

            builder.Property( a => a.PatientId )
                .IsRequired();

            builder.Property( a => a.PatientFirstName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.PatientSecondName )
                .IsRequired()
                .HasMaxLength( 80 );

            builder.Property( a => a.StartTime )
                .IsRequired();

            builder.Property( a => a.Duration )
                .IsRequired();

        }
    }
    public class ResultConfiguration: IEntityTypeConfiguration<Result> {
        public void Configure( EntityTypeBuilder<Result> builder ) {
            builder.HasKey( a => a.Id );

            builder.Property( a => a.Conclusion )
                .HasMaxLength(3000)
                .IsRequired();

            builder.Property( a => a.DocumentUrl )
                .IsRequired()
                .HasMaxLength( 300 );

            builder.Property( a => a.Complaints)
                .IsRequired()
                .HasMaxLength( 500 );

            builder.Property( a => a.Recomendations )
                .IsRequired()
                .HasMaxLength( 1000 );

        }
    }

}
