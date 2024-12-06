using AutoMapper;
using Offices.Application.Dtos;
using Offices.Domain.Models;

namespace Offices.Application.AutoMapper {
    public class OfficeProfile: Profile {
        public OfficeProfile() {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Office, OfficeDto>().ReverseMap();
            CreateMap<Office, UpdateOfficeDto>().ReverseMap();
            CreateMap<Office, CreateOfficeDto>().ReverseMap();

        }
    }
}
