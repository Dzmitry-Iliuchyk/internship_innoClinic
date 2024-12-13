using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Profiles.Domain;
using System.Data;

namespace Profiles.DataAccess {
    public class ProfilesDbContext: DbContext {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public ProfilesDbContext( DbContextOptions options ) : base( options ) {
        }
        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<Account>()
                .HasIndex( x => x.Email )
                .IsUnique( true );
            modelBuilder.Entity<Account>()
                .Property(x=>x.Email)
                .HasMaxLength(120)
                .IsUnicode( true );
            modelBuilder.Entity<Account>()
                .Property(x=>x.FirstName)
                .HasMaxLength(80)
                .IsUnicode( true );
            modelBuilder.Entity<Account>()
                .Property(x=>x.LastName)
                .HasMaxLength(80)
                .IsUnicode( true );
            modelBuilder.Entity<Account>()
                .Property(x=>x.MiddleName)
                .HasMaxLength(80)
                .IsUnicode( true );
            modelBuilder.Entity<Account>()
                .Property(x=>x.PhoneNumber)
                .HasMaxLength(25)
                .IsUnicode( true );
            modelBuilder.Entity<Account>()
                .Property(x=>x.PhotoUrl)
                .HasMaxLength(255)
                .IsUnicode(true);


            modelBuilder.Entity<Specialization>()
                .Property(x=>x.Name)
                .HasMaxLength(120)
                .IsUnicode(true);
            modelBuilder.Entity<Specialization>()
                .Property(x=>x.Id).ValueGeneratedNever();

            modelBuilder.Entity<Account>().UseTptMappingStrategy();
        }

        public void EnsureSeedData() {
            if (!Doctors.Any()) {
                var faker = new Faker();

                var specializations = Enum.GetValues<Specializations>().Select( x => new Specialization {
                    Id = (int)x,
                    Name = x.ToString(),
                    isActive = true
                } );
                this.Specializations.AddRange( specializations );

                var doctors = new List<Doctor>();
                var receptionists = new List<Receptionist>();
                var patients = new List<Patient>();

                for (int i = 0; i < 10; i++) {
                    doctors.Add( new Doctor(
                   id: Guid.NewGuid(),
                   dateOfBirth: faker.Date.Past( 50, DateTime.UtcNow ),
                   careerStartYear: faker.Date.Past( 20, DateTime.UtcNow ),
                   officeId: faker.Commerce.Department(),
                   status: faker.PickRandom<DoctorStatuses>(),
                   specializationId: faker.PickRandom( specializations ).Id,
                   firstName: faker.Name.FirstName(),
                   lastName: faker.Name.LastName(),
                   email: faker.Internet.Email(),
                   phoneNumber: faker.Phone.PhoneNumber(),
                   isEmailVerified: true,
                   createdBy: Guid.NewGuid(),
                   createdAt: DateTime.UtcNow,
                   updatedBy: Guid.NewGuid(),
                   updatedAt: DateTime.UtcNow,
                   photoUrl: faker.Internet.Avatar(),
                   middleName: faker.Person.UserName
               ) );

                    receptionists.Add( new Receptionist(
                        id: Guid.NewGuid(),
                        officeId: faker.Commerce.Department(),
                        firstName: faker.Name.FirstName(),
                        lastName: faker.Name.LastName(),
                        email: faker.Internet.Email(),
                        phoneNumber: faker.Phone.PhoneNumber(),
                        isEmailVerified: true,
                        createdBy: Guid.NewGuid(),
                        createdAt: DateTime.UtcNow,
                        updatedBy: Guid.NewGuid(),
                        updatedAt: DateTime.UtcNow,
                        photoUrl: faker.Internet.Avatar(),
                        middleName: faker.Person.UserName
                    ) );

                    patients.Add( new Patient(
                        id: Guid.NewGuid(),
                        dateOfBirth: faker.Date.Past( 30, DateTime.UtcNow ),
                        firstName: faker.Name.FirstName(),
                        lastName: faker.Name.LastName(),
                        email: faker.Internet.Email(),
                        phoneNumber: faker.Phone.PhoneNumber(),
                        isEmailVerified: true,
                        createdBy: Guid.NewGuid(),
                        createdAt: DateTime.UtcNow,
                        updatedBy: Guid.NewGuid(),
                        updatedAt: DateTime.UtcNow,
                        photoUrl: faker.Internet.Avatar(),
                        middleName: null
                    ) );
                }

                Doctors.AddRange( doctors );
                Receptionists.AddRange( receptionists );
                Patients.AddRange( patients );
                SaveChanges();
            }
        }
    }
    public enum Specializations {
        None = 1,
        Therapist,
        Nurse,
        Ophthalmologist
    }



}
