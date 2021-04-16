using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Configuration;

public class DailyEntryService
{
    private readonly IMongoCollection<DailyEntry> _dailyEntries;
    IConfiguration _configuration;
    public DailyEntryService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        var client = new MongoClient(_configuration["DBCONNECTION"]);
        var database = client.GetDatabase(_configuration["DBNAME"]);
        _dailyEntries = database.GetCollection<DailyEntry>(_configuration["DBCOLLECTION"]);
    }

    public async Task<List<DailyEntry>> GetAllAsync()
    {
        return await _dailyEntries.Find(entry => true).ToListAsync();
    }

    public async Task<List<DailyEntry>> GetAllByUserAsync(string token)
    {
        return await _dailyEntries.Find(entry => entry.Token == token).SortBy(entry => entry.Date).ToListAsync();
        //return await _dailyEntries.Find(entry => entry.Token == token).Sort("{Date: 1}").ToListAsync();
    }


    public async Task<DailyEntry> GetByIdAsync(string id)
    {
        return await _dailyEntries.Find<DailyEntry>(entry => entry.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<DailyEntry>> GetByDateAndUserAsync(string token, string date)
    {
        return await _dailyEntries.Find<DailyEntry>(entry => entry.Date == date && entry.Token == token).ToListAsync();
    }

    public async Task<List<DailyEntry>> GetBySearchAndUserAsync(string token, string search)  //!!! to improve
    {

        // var F = Builders<DailyEntry>.Filter.Text($"{search}");
        // var P = Builders<DailyEntry>.Projection.MetaTextScore("TextMatchScore");
        // var S = Builders<DailyEntry>.Sort.MetaTextScore("TextMatchScore");
        //return await _dailyEntries.Find(F).Sort(S).ToListAsync();

        var filter = Builders<DailyEntry>.Filter;
        var searchFilter = filter.Text($"{search}");
        var tokenFilter = filter.Eq(entry => entry.Token, token);
        var finalFilter = filter.And(tokenFilter, searchFilter);

        return await _dailyEntries.Find<DailyEntry>(finalFilter).ToListAsync();  //sort of working - full words
        
        //return await _dailyEntries.Find<DailyEntry>(entry => entry.Token == token && entry.Topics.Contains(search)).ToListAsync();      //sort of working - full words
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