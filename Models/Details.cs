namespace IMDB_API.Models
{
    public class Details
    {
        public string @type { get; set; }
        public string id { get; set; }
        public Image image { get; set; }
        public int runningTimeInMinutes { get; set; }
        public string title { get; set; }
        public string titleType { get; set; }
        public int year { get; set; }
    }
}
