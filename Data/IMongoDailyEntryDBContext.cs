using MongoDB.Driver;
public interface IMongoDailyEntryDBContext
{
    IMongoCollection<DailyEntry> GetCollection<DailyEntry>(string name);

}