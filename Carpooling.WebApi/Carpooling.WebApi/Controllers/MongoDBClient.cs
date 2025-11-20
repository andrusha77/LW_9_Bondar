using MongoDB.Driver;

namespace Carpooling.WebApi.Controllers
{
    

    public class MongoDBClient
    {
        private static IMongoDatabase _db;
        private static MongoDBClient _instance;

        public static MongoDBClient Instance
        {
            get => _instance ?? new MongoDBClient();
        }

        private MongoDBClient()
        {
            var connectionString = "mongodb+srv://bondandriy2008_db_user:2008@cluster0.tv1fu1o.mongodb.net/?appName=Cluster0"; // або URI від MongoDB Atlas
            var client = new MongoClient(connectionString);
            _db = client.GetDatabase("mydatabase");

        }

        public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);
    }
}
