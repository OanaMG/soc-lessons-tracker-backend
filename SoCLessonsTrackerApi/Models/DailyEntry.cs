using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

public class DailyEntry
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string Date { get; set; }

    [Required]
    public string Topics { get; set; }
    public string NotionLinks { get; set; }
    public string AdditionalResourcesLinks { get; set; }
    public string GithubLinks { get; set; }
    public string AdditionalNotes { get; set; }

    [Required]
    [Range(0, 10)]
    public int RecapQuizScore { get; set;}
    public string Token { get; set; }
    public string[] UploadedDocuments { get; set; }           

}
