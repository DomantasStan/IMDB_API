namespace IMDB_API.Models
{
    public class MovieRating
    {
        public string id { get; set; }
        public double rating { get; set; }
        public string title { get; set; }
        public string genre { get; set; }

        public MovieRating(string id, double rating, string title, string genre)
        {
            this.id = id;
            this.rating = rating;
            this.title = title;
            this.genre = genre;
        }
    }
}
