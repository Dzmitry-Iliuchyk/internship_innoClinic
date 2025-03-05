using Dapper;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Abstractions.Entities;

namespace Profiles.DataAccess.RepositoriesDapper {
    public class DoctorReadRepository: IDoctorReadRepository {
        private readonly DapperProfileContext _context;
        public DoctorReadRepository( DapperProfileContext context ) {
            _context = context;
        }
        public async Task<Doctor?> GetAsync( Guid id ) {
            const string sql = @"
                SELECT a.Id, a.FirstName, a.LastName, a.MiddleName, a.Email, 
                       a.PhoneNumber, a.IsEmailVerified, a.PhotoUrl, a.CreatedBy, a.CreatedAt, a.UpdatedBy, a.UpdatedAt,
                       d.DateOfBirth, d.CareerStartYear, d.OfficeId, d.Status, d.SpecializationId,
                       s.Id AS SpecializationId, s.Id, s.Name, s.isActive
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
        public async Task<PagedResult<Doctor>?> GetPageAsync(int skip, int take) {
            const string sql = """
                        SELECT a.*, d.*, s.*
                        FROM Accounts a
                        JOIN Doctors d ON a.Id = d.Id
                        JOIN Specializations s ON d.SpecializationId = s.Id
                        ORDER BY a.Id
                        OFFSET     @skip ROWS
                        FETCH NEXT @take ROWS ONLY;
                        """;
            const string sqlCountQuery = """
                        SELECT COUNT(*) 
                        FROM Accounts a
                        JOIN Doctors d ON a.Id = d.Id
                        JOIN Specializations s ON d.SpecializationId = s.Id;
                        """;
            var param = new DynamicParameters();
            param.Add( "skip", skip, System.Data.DbType.Int32 );
            param.Add( "take", take, System.Data.DbType.Int32 );
            using (var connection = _context.CreateConnection()) {
                var count = await connection.QuerySingleAsync<int>( sqlCountQuery );

                var doctors = await connection.QueryAsync<Doctor, Specialization, Doctor>( sql,
                    ( d, s ) => {
                        d.Specialization = s;
                        return d;
                    }, 
                    param,
                    splitOn: "Id" );

                return new PagedResult<Doctor>( totalCount : count  , items: doctors.ToList());
            }
        }
    }
}
