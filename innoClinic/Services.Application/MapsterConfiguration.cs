using Mapster;
using Services.Application.Abstractions.Services;
using Services.Domain;

namespace Services.Application {
    public static class MapsterConfiguration {
        public static void Configure() {
            #region service
            TypeAdapterConfig<Service, ServiceDto>.NewConfig()
                .Map( dest => dest.SpecializationName, src => src.Specialization.Name )
                .Map( dest => dest.CategoryName, src => src.Category.Name ); 
            TypeAdapterConfig<CreateServiceDto, Service >.NewConfig()
                .Map( dest => dest.Specialization.Name, src => src.SpecializationName )
                .Map( dest => dest.Category.Name, src => src.CategoryName);
            TypeAdapterConfig<UpdateServiceDto, Service >.NewConfig()
                .Map( dest => dest.Specialization.Name, src => src.SpecializationName )
                .Map( dest => dest.Category.Name, src => src.CategoryName);
            #endregion service
        }
    }
}
