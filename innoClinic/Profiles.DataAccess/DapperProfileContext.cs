using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Profiles.DataAccess {
    public class DapperProfileContext {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperProfileContext( IConfiguration configuration ) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString( "DapperProfiles" )??throw new Exception();
        }


        public IDbConnection CreateConnection()
            => new SqlConnection( _connectionString );
    }
}
