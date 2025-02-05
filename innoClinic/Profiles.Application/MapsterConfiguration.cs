using Mapster;
using Profiles.Application.Doctors.Commands.Create;
using Profiles.Application.Doctors.Commands.Update;
using Profiles.Application.Doctors.Queries;
using Profiles.Application.Patients.Commands.Create;
using Profiles.Application.Patients.Commands.Update;
using Profiles.Application.Receptionists.Commands.Create;
using Profiles.Application.Receptionists.Commands.Update;
using Profiles.Domain;

namespace Profiles.Application {
    public static class MapsterConfiguration {
        public static void Configure() {
            #region doctor
            TypeAdapterConfig<Doctor, DoctorDto>.NewConfig()
                .Map( dest => dest.Specialization, src => src.Specialization.Name );

            TypeAdapterConfig<CreateDoctorCommand, Doctor>.NewConfig()
                .MapWith( src => new Doctor(
                    Guid.NewGuid(),
                    src.DateOfBirth,
                    src.CareerStartYear,
                    src.OfficeId,
                    src.Status,
                    src.SpecializationId,
                    src.FirstName,
                    src.LastName,
                    src.Email,
                    src.PhoneNumber,
                    false,
                    src.CreatedBy,
                    DateTime.UtcNow,
                    src.CreatedBy,
                    DateTime.UtcNow,
                    src.PhotoUrl,
                    src.MiddleName) );


            TypeAdapterConfig<(UpdateDoctorCommand, Doctor), Doctor>.NewConfig().MapWith( src => new Doctor(
                Guid.NewGuid(),
                src.Item1.DateOfBirth,
                src.Item1.CareerStartYear,
                src.Item1.OfficeId,
                src.Item1.Status,
                src.Item1.SpecializationId,
                src.Item1.FirstName,
                src.Item1.LastName,
                src.Item1.Email,
                src.Item1.PhoneNumber,
                src.Item2.IsEmailVerified,
                src.Item2.CreatedBy,
                src.Item2.CreatedAt,
                src.Item1.UpdatedBy,
                DateTime.UtcNow,
                src.Item1.PhotoUrl,
                src.Item1.MiddleName
                ) );
            #endregion doctor
            #region patient
            TypeAdapterConfig<CreatePatientCommand, Patient>.NewConfig().MapWith( src => new Patient(
                Guid.NewGuid(),
                src.DateOfBirth,
                src.FirstName,
                src.LastName,
                src.Email,
                src.PhoneNumber,
                false,
                src.CreatedBy,
                DateTime.UtcNow,
                src.CreatedBy,
                DateTime.UtcNow,
                src.PhotoUrl,
                src.MiddleName
            ) );
            
            TypeAdapterConfig<(UpdatePatientCommand,Patient), Patient>.NewConfig().MapWith( src => new Patient(
                src.Item1.Id,
                src.Item1.DateOfBirth,
                src.Item1.FirstName,
                src.Item1.LastName,
                src.Item1.Email,
                src.Item1.PhoneNumber,
                src.Item2.IsEmailVerified,
                src.Item2.CreatedBy,
                src.Item2.CreatedAt,
                src.Item1.UpdatedBy,
                DateTime.UtcNow,
                src.Item1.PhotoUrl,
                src.Item1.MiddleName
            ) );
            #endregion patient
            #region Receptionist
            TypeAdapterConfig<CreateReceptionistCommand, Receptionist>.NewConfig().MapWith( src => new Receptionist(
                Guid.NewGuid(),
                src.OfficeId,
                src.FirstName,
                src.LastName,
                src.Email,
                src.PhoneNumber,
                false,
                src.CreatedBy,
                DateTime.UtcNow,
                src.CreatedBy,
                DateTime.UtcNow,
                src.PhotoUrl,
                src.MiddleName
            ) );


            TypeAdapterConfig<(UpdateReceptionistCommand, Receptionist), Receptionist>.NewConfig()
                .MapWith( src => new Receptionist(
                    src.Item1.Id,
                    src.Item1.OfficeId,
                    src.Item1.FirstName,
                    src.Item1.LastName,
                    src.Item1.Email,
                    src.Item1.PhoneNumber,
                    src.Item2.IsEmailVerified,
                    src.Item2.CreatedBy,
                    src.Item2.CreatedAt,
                    src.Item1.UpdatedBy,
                    DateTime.UtcNow,
                    src.Item1.PhotoUrl,
                    src.Item1.MiddleName
            ) );
            #endregion Receptionist
        }
    }
}