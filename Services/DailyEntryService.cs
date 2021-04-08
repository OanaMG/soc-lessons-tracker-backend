using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class DailyEntryService
{
    private readonly IMongoCollection<DailyEntry> _dailyEntries;

    public DailyEntryService(ISoCLessonsTrackerDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _dailyEntries = database.GetCollection<DailyEntry>(settings.DailyJournalEntriesCollectionName);
    }

    public async Task<List<DailyEntry>> GetAllAsync()
    {
        return await _dailyEntries.Find(entry => true).ToListAsync();
    }

    public async Task<DailyEntry> GetByIdAsync(string id)
    {
        return await _dailyEntries.Find<DailyEntry>(entry => entry.Id == id).FirstOrDefaultAsync();
    }

    public async Task<DailyEntry> CreateAsync(DailyEntry dailyEntry)
    {
        await _dailyEntries.InsertOneAsync(dailyEntry);
        return dailyEntry;
    }

    public async Task UpdateAsync(string id, DailyEntry dailyEntry)
    {
        await _dailyEntries.ReplaceOneAsync(entry => entry.Id == id, dailyEntry);
    }

    public async Task DeleteAsync(string id)
    {
        await _dailyEntries.DeleteOneAsync(entry => entry.Id == id);
    }
}