using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DailyEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Date { get; set; }
    public string Topics { get; set; }
    public string NotionLinks { get; set; }
    public string AdditionalResourcesLinks { get; set; }
    public string AdditionalNotes { get; set; }
    public int RecapQuizScore { get; set;}
}