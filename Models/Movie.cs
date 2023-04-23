using Microsoft.EntityFrameworkCore;

namespace IMDB_API.Models
{
    public class Movie
    {
        
        public string id{ get; set; }

        public double chartRating { get; set; }

        public Movie(string id, double chartRating)
        {
            this.id = id;
            this.chartRating = chartRating;
        }
    }
}
