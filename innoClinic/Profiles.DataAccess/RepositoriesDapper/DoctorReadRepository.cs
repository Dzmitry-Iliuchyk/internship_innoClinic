using Dapper;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;

namespace Profiles.DataAccess.RepositoriesDapper {
    public class DoctorReadRepository: IDoctorReadRepository {
        private readonly DapperProfileContext _context;
        public DoctorReadRepository( DapperProfileContext context ) {
            _context = context;
        }
        public async Task<Doctor?> GetAsync( Guid id ) {
            const string sql = @"
                SELECT a.Id, a.FirstName, a.SecondName, a.MiddleName, a.Email, 
                       a.PhoneNumber, a.IsEmailVerified, a.PhotoUrl, a.CreatedBy, a.CreatedAt, a.UpdatedBy, a.UpdatedAt,
                       d.DateOfBirth, d.CareerStartYear, d.OfficeId, d.Status, d.SpecializationId,
                       s.Id AS SpecializationId, s.Name, s.isActive
                FROM Accounts a
                JOIN Doctors d ON a.Id = d.Id
                JOIN Specializations s ON d.SpecializationId = s.Id
                WHERE d.Id = @doctorId";

            var param = new DynamicParameters();
            param.Add( "doctorId", id, System.Data.DbType.Guid );
            using (var connection = _context.CreateConnection()) {
                var doctor = await connection.QueryAsync<Doctor, Specialization, Doctor>(
                    sql,
                    ( doctor, specialization ) => {
                        doctor.Specialization = specialization;
                        return doctor;
                    },
                    param,
                    splitOn: "SpecializationId"
                );

                return doctor.SingleOrDefault();
            }
        }
        public async Task<IList<Doctor>?> GetAllAsync() {
            const string sql = """
                        SELECT a.*, d.*, s.*
                        FROM Accounts a
                        JOIN Doctors d ON a.Id = d.Id
                        JOIN Specializations s ON d.SpecializationId = s.Id
                        """;

            using (var connection = _context.CreateConnection()) {
                var doctors = await connection.QueryAsync<Doctor, Specialization, Doctor>( sql,
                    ( d, s ) => {
                        d.Specialization = s;
                        return d;
                    }, splitOn: "Id" );

                return doctors.ToList();
            }
        }
    }
}
