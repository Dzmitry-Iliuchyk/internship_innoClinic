using Microsoft.EntityFrameworkCore;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Abstractions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.DataAccess.Repositories {
    public class DoctorCommandRepository: BaseRepository<Doctor>, IDoctorCommandRepository {
        public DoctorCommandRepository( ProfilesDbContext context ) : base( context ) {
        }
    }
}
