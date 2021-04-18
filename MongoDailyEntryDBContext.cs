using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class MongoDailyEntryDBContext : IMongoDailyEntryDBContext
{
    private IMongoDatabase _db { get; set; }
    private MongoClient _mongoClient { get; set; }
    public IClientSessionHandle Session { get; set; }
    IConfiguration _configuration;
    public MongoDailyEntryDBContext(IConfiguration configuration)  
    {
        _configuration = configuration;
        _mongoClient = new MongoClient(_configuration["DBCONNECTION"]);
        _db = _mongoClient.GetDatabase(_configuration["DBNAME"]);
    }
  
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _db.GetCollection<T>(name);
    }
}