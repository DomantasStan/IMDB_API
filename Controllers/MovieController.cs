using IMDB_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace IMDB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private string? connectionString;

        public MovieController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("MovieCS");
        }

        [HttpGet]
        public async Task<IActionResult> getGenres()
        {
            using var connection = new MySqlConnection(connectionString);
            string sql = "SELECT * FROM genres;";
            await connection.OpenAsync();

            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            List<string> genres = new List<string>();

            while (await reader.ReadAsync())
            {
                string value = reader.GetValue(0).ToString();
                genres.Add(value);
            }

            return Ok(genres);
        }

        [HttpGet]
        [Route("{genre}")]
        public async Task<IActionResult> getMovies([FromRoute] string genre)
        {
            using var connection = new MySqlConnection(connectionString);
            string sql = "SELECT * FROM `movie_ratings` WHERE `genre` LIKE \"%" + genre + "%\" ORDER BY `rating` DESC";
            await connection.OpenAsync();

            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            List<MovieRating> moviesRatings = new List<MovieRating>();

            while (await reader.ReadAsync())
            {
                string value = reader.GetValue(2).ToString();
                moviesRatings.Add(new MovieRating(reader.GetValue(0).ToString(), Convert.ToDouble(reader.GetValue(1).ToString()),
                    reader.GetValue(2).ToString(), reader.GetValue(3).ToString()));
            }

            if (moviesRatings.Count < 1)
            {
                return NotFound("There are no movies for the specified genre of yours");
            }

            return Ok(moviesRatings);
        }
    }
}