using AutoMapper;
using Offices.DataAccess.Models;
using Offices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.DataAccess.Mapper {
    public class OfficeProfile: Profile {
        public OfficeProfile() {
            CreateMap<Address, AddressEntity>().ReverseMap();
            CreateMap<Office, OfficeEntity>().ReverseMap();
            
        }
    }
}
