using Appointments.Application.Dtos;
using Appointments.Domain;
using Mapster;

namespace Appointments.Application {
    public static class MapsterConfiguration {
        public static void Configure() {

            TypeAdapterConfig<Appointment, AppointmentDto>.NewConfig()
                .Map( dest => dest.Id, src => src.Id )
                .Map( dest => dest.DoctorFirstName, src => src.Doctor.DoctorFirstName )
                .Map( dest => dest.DoctorSecondName, src => src.Doctor.DoctorSecondName )
                .Map( dest => dest.DoctorSpecialization, src => src.Doctor.DoctorSpecialization )
                .Map( dest => dest.ServiceName, src => src.Service.ServiceName )
                .Map( dest => dest.ServicePrice, src => src.Service.ServicePrice )
                .Map( dest => dest.OfficeAddress, src => src.Office.OfficeAddress )
                .Map( dest => dest.PatientFirstName, src => src.Patient.PatientFirstName )
                .Map( dest => dest.PatientSecondName, src => src.Patient.PatientSecondName )
                .Map( dest => dest.PatientEmail, src => src.Patient.PatientEmail )
                .Map( dest => dest.StartTime, src => src.StartTime )
                .Map( dest => dest.Duration, src => src.Duration )
                .TwoWays();

            TypeAdapterConfig<AppointmentCreateDto, Appointment>.NewConfig()
                .Map( dest => dest.DoctorId, src => src.DoctorId )
                .Map( dest => dest.PatientId, src => src.PatientId )
                .Map( dest => dest.OfficeId, src => src.OfficeId )
                .Map( dest => dest.ServiceId, src => src.ServiceId )
                .Map( dest => dest.StartTime, src => src.StartTime )
                .Map( dest => dest.Duration, src => src.Duration );

            TypeAdapterConfig<AppointmentUpdateDto, Appointment>.NewConfig()
                .Map( dest => dest.DoctorId, src => src.DoctorId )
                .Map( dest => dest.PatientId, src => src.PatientId )
                .Map( dest => dest.OfficeId, src => src.OfficeId )
                .Map( dest => dest.ServiceId, src => src.ServiceId )
                .Map( dest => dest.StartTime, src => src.StartTime )
                .Map( dest => dest.Duration, src => src.Duration );
                //.TwoWays();

            

        }
    }
}
