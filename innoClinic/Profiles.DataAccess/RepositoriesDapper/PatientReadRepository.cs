using Dapper;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;

namespace Profiles.DataAccess.RepositoriesDapper {
    public class PatientReadRepository: IPatientReadRepository {
        private readonly DapperProfileContext _context;
        public PatientReadRepository( DapperProfileContext context ) {
            _context = context;
        }
        public async Task<Patient?> GetAsync( Guid id ) {
            const string sql = @" 
                    SELECT a.Id,
                           p.DateOfBirth,
                           a.FirstName,
                           a.LastName,
                           a.Email,
                           a.PhoneNumber,
                           a.IsEmailVerified,
                           a.CreatedBy,
                           a.CreatedAt,
                           a.UpdatedBy,
                           a.UpdatedAt,
                           a.PhotoUrl,
                           a.MiddleName
                    FROM Accounts a 
                    JOIN Patients p ON a.Id = p.Id 
                    WHERE p.Id = @patientId
                        ";
            var param = new DynamicParameters();
            param.Add( "patientId", id, System.Data.DbType.Guid );
            using (var connection = _context.CreateConnection()) {
                var patient = await connection.QuerySingleOrDefaultAsync<Patient>( sql, param );
                return patient;
            }
        }
        public async Task<IList<Patient>?> GetAllAsync() {
            const string sql = @" 
                    SELECT a.Id,
                           p.DateOfBirth,
                           a.FirstName,
                           a.LastName,
                           a.Email,
                           a.PhoneNumber,
                           a.IsEmailVerified,
                           a.CreatedBy,
                           a.CreatedAt,
                           a.UpdatedBy,
                           a.UpdatedAt,
                           a.PhotoUrl,
                           a.MiddleName
                    FROM Accounts a 
                    JOIN Patients p ON a.Id = p.Id 
                        ";
            using (var connection = _context.CreateConnection()) {
                var patient = await connection.QueryAsync<Patient>( sql );

                return patient.ToList();
            }
        }
    }
    public class AccountReadRepository: IAccountReadRepository {
        private readonly DapperProfileContext _context;
        public AccountReadRepository( DapperProfileContext context ) {
            _context = context;
        }

        public async Task<string> GetPathToImage( Guid id ) {
            const string sql = @" 
                    SELECT a.PhotoUrl
                    FROM Accounts a 
                    WHERE a.Id = @accId
                        ";
            var param = new DynamicParameters();
            param.Add( "accId", id, System.Data.DbType.Guid );
            using (var connection = _context.CreateConnection()) {
                var imagePath = await connection.QuerySingleOrDefaultAsync<string>( sql, param )??"";
                return imagePath;
            }
        }

    }
}
