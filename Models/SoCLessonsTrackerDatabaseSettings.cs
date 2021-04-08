public class SoCLessonsTrackerDatabaseSettings : ISoCLessonsTrackerDatabaseSettings
{
    public string DailyJournalEntriesCollectionName { get; set; }
    // public string CoursesCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}

public interface ISoCLessonsTrackerDatabaseSettings
{
    string DailyJournalEntriesCollectionName { get; set; }
    // string CoursesCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}