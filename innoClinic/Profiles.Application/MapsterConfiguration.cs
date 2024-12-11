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
                    src.dateOfBirth,
                    src.careerStartYear,
                    src.officeId,
                    src.status,
                    src.specializationId,
                    src.firstName,
                    src.lastName,
                    src.email,
                    src.phoneNumber,
                    false,
                    src.createdBy,
                    DateTime.UtcNow,
                    src.createdBy,
                    DateTime.UtcNow,
                    src.photoUrl,
                    src.middleName) );


            TypeAdapterConfig<(UpdateDoctorCommand, Doctor), Doctor>.NewConfig().MapWith( src => new Doctor(
                Guid.NewGuid(),
                src.Item1.dateOfBirth,
                src.Item1.careerStartYear,
                src.Item1.officeId,
                src.Item1.status,
                src.Item1.specializationId,
                src.Item1.firstName,
                src.Item1.lastName,
                src.Item1.email,
                src.Item1.phoneNumber,
                src.Item2.IsEmailVerified,
                src.Item2.CreatedBy,
                src.Item2.CreatedAt,
                src.Item1.updatedBy,
                DateTime.UtcNow,
                src.Item1.photoUrl,
                src.Item1.middleName
                ) );
            #endregion doctor
            #region patient
            TypeAdapterConfig<CreatePatientCommand, Patient>.NewConfig().MapWith( src => new Patient(
                Guid.NewGuid(),
                src.dateOfBirth,
                src.firstName,
                src.lastName,
                src.email,
                src.phoneNumber,
                false,
                src.createdBy,
                DateTime.UtcNow,
                src.createdBy,
                DateTime.UtcNow,
                src.photoUrl,
                src.middleName
            ) );
            
            TypeAdapterConfig<(UpdatePatientCommand,Patient), Patient>.NewConfig().MapWith( src => new Patient(
                src.Item1.id,
                src.Item1.dateOfBirth,
                src.Item1.firstName,
                src.Item1.lastName,
                src.Item1.email,
                src.Item1.phoneNumber,
                src.Item2.IsEmailVerified,
                src.Item2.CreatedBy,
                src.Item2.CreatedAt,
                src.Item1.updatedBy,
                DateTime.UtcNow,
                src.Item1.photoUrl,
                src.Item1.middleName
            ) );
            #endregion patient
            #region Receptionist
            TypeAdapterConfig<CreateReceptionistCommand, Receptionist>.NewConfig().MapWith( src => new Receptionist(
                Guid.NewGuid(),
                src.officeId,
                src.firstName,
                src.lastName,
                src.email,
                src.phoneNumber,
                false,
                src.createdBy,
                DateTime.UtcNow,
                src.createdBy,
                DateTime.UtcNow,
                src.photoUrl,
                src.middleName
            ) );


            TypeAdapterConfig<(UpdateReceptionistCommand, Receptionist), Receptionist>.NewConfig()
                .MapWith( src => new Receptionist(
                    src.Item1.id,
                    src.Item1.officeId,
                    src.Item1.firstName,
                    src.Item1.lastName,
                    src.Item1.email,
                    src.Item1.phoneNumber,
                    src.Item2.IsEmailVerified,
                    src.Item2.CreatedBy,
                    src.Item2.CreatedAt,
                    src.Item1.updatedBy,
                    DateTime.UtcNow,
                    src.Item1.photoUrl,
                    src.Item1.middleName
            ) );
            #endregion Receptionist
        }
    }
}