using FacadeApi.Offices.Dtos;

namespace FacadeApi.Offices {
    public static class Extensions {
        public static UpdateOfficeDtoForApi ToUpdateOfficeDtoForApi( this UpdateOfficeDto updateOfficeDto ) {
            var updateOfficeDtoForApi = new UpdateOfficeDtoForApi() {
                 Address = updateOfficeDto.Address,
                 RegistryPhoneNumber = updateOfficeDto.RegistryPhoneNumber,
                 Status = updateOfficeDto.Status,
            };
            if (updateOfficeDto.Photo!=null) {
                updateOfficeDtoForApi.PhotoUrl = $"office:{updateOfficeDto.RegistryPhoneNumber}-{updateOfficeDto.Photo.Name}";
            }
            return updateOfficeDtoForApi;
        }
        public static CreateOfficeDtoForApi ToCreateOfficeDtoForApi( this CreateOfficeDto updateOfficeDto ) {
            var createOfficeDtoForApi = new CreateOfficeDtoForApi() {
                Address = updateOfficeDto.Address,
                RegistryPhoneNumber = updateOfficeDto.RegistryPhoneNumber,
                Status = updateOfficeDto.Status,
            };
            if (updateOfficeDto.Photo != null) {
                createOfficeDtoForApi.PhotoUrl = $"office:{updateOfficeDto.RegistryPhoneNumber}-{updateOfficeDto.Photo.Name}";
            }
            return createOfficeDtoForApi;
        }
    }
}
