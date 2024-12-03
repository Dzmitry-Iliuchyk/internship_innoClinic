using MongoDB.Bson;
using Offices.Application.Interfaces.Services;

namespace Offices.DataAccess.Services {
    public class MongoIdGenerator: IIdGenerator {
        public string GenerateId() {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
