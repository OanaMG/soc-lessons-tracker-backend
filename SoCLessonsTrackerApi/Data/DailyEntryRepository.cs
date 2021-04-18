using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Configuration;


public class DailyEntryRepository: IRepository<DailyEntry>
{       
    IConfiguration _configuration;
    private readonly IMongoDailyEntryDBContext _mongoContext;
    private readonly IMongoCollection<DailyEntry> _dailyEntries;
 
    public DailyEntryRepository(IMongoDailyEntryDBContext context, IConfiguration configuration)  
        {  
            _configuration = configuration;
            _mongoContext = context;
            _dailyEntries = _mongoContext.GetCollection<DailyEntry>(_configuration["DBCOLLECTION"]);
        }  

    public async Task<List<DailyEntry>> GetAllAsync()
    {
        return await _dailyEntries.Find(entry => true).ToListAsync();
    }

    public async Task<List<DailyEntry>> GetAllByUserAsync(string token)
    {
        return await _dailyEntries.Find(entry => entry.Token == token).SortBy(entry => entry.Date).ToListAsync();
    }


    public async Task<DailyEntry> GetByIdAsync(string id)
    {
        return await _dailyEntries.Find<DailyEntry>(entry => entry.Id == id).FirstOrDefaultAsync();
    }

    public async Task<DailyEntry> GetByDateAndUserAsync(string token, string date)
    {
        return await _dailyEntries.Find<DailyEntry>(entry => entry.Date == date && entry.Token == token).FirstOrDefaultAsync();
    }

    public async Task<List<DailyEntry>> GetBySearchAndUserAsync(string token, string search)
    {
        var filter = Builders<DailyEntry>.Filter;
        var searchOptions = new MongoDB.Driver.TextSearchOptions();
        searchOptions.CaseSensitive = false;
        var searchFilter = filter.Text($"{search}", searchOptions);
        var tokenFilter = filter.Eq(entry => entry.Token, token);
        var finalFilter = filter.And(tokenFilter, searchFilter);
        return await _dailyEntries.Find<DailyEntry>(finalFilter).ToListAsync();
    }

    public async Task<DailyEntry> InsertAsync(DailyEntry dailyEntry)
    {
        await _dailyEntries.InsertOneAsync(dailyEntry);
        return dailyEntry;
    }

    public async Task<DailyEntry> UpdateAsync(string id, DailyEntry dailyEntry)
    {
        await _dailyEntries.ReplaceOneAsync(entry => entry.Id == id, dailyEntry);
        return dailyEntry;
    }

    public async void DeleteAsync(string id)
    {
        await _dailyEntries.DeleteOneAsync(entry => entry.Id == id);
    }
}