using Dapper;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System.Linq.Expressions;

namespace Profiles.DataAccess.RepositoriesDapper {
    public class ReceptionistReadRepository: IReceptionistReadRepository {
        private readonly DapperProfileContext _context;
        public ReceptionistReadRepository( DapperProfileContext context ) {
            _context = context;
        }
        public async Task<Receptionist?> GetAsync( Guid id ) {
            const string sql = @"
                    SELECT a.Id,
                           r.OfficeId,
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
                    JOIN Receptionists r ON a.Id = r.Id 
                    WHERE r.Id = @receptionistId
                        ";
            var param = new DynamicParameters();
            param.Add( "receptionistId", id, System.Data.DbType.Guid );
            using (var connection = _context.CreateConnection()) {
                var patient = await connection.QuerySingleOrDefaultAsync<Receptionist>( sql, param );

                return patient;
            }
        }
        public async Task<IList<Receptionist>?> GetAllAsync() {
            const string sql = @" 
                    SELECT a.Id,
                           r.OfficeId,
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
                    JOIN Receptionists r ON a.Id = r.Id 
                        ";

            using (var connection = _context.CreateConnection()) {
                var receptionist = await connection.QueryAsync<Receptionist>( sql );

                return receptionist.ToList();
            }
        }
    }
}
