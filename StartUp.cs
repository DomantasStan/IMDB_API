using IMDB_API.Models;
using MySqlConnector;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json;

namespace IMDB_API
{
    public class StartUp
    {
        private string connectionString;

        public StartUp(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void PrintTest(int count)
        {
            Console.WriteLine("Test " + count);
        }

        // Removes unnecessary data from id value
        public string parseString(string id)
        {
            return id.Split('/')[2];
        }

        // Removes unnecessary symbols from genres
        public string parseGenres(string genre)
        {
            return genre.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("\\", "");
        }

        // Gets unique genres
        public List<string> getUniqueGenres(List<string> genres)
        {
            List<string> uniqueGenres = new List<string>();
            foreach (string genre in genres)
            {
                foreach (string gen in genre.Split(","))
                {
                    if (!uniqueGenres.Contains(gen))
                    {
                        uniqueGenres.Add(gen);
                    }
                }
            }
            return uniqueGenres;
        }

        // Posts unique genres to database
        public async void postGenres(List<string> genres)
        {
            string sql = "DELETE FROM `genres`; ";

            genres.ForEach(genre => {
                sql += "INSERT INTO genres VALUES(\"" + genre + "\"); ";
            });

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Creates table required for data insertion to database
        public async void createTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS movie_ratings (id VARCHAR(255) NOT NULL," +
                " rating double(2,1) DEFAULT NULL, title VARCHAR(255) DEFAULT NULL, genre VARCHAR(255) DEFAULT NULL);" +
                "\r\nCREATE TABLE IF NOT EXISTS genres (title VARCHAR(255) NOT NULL);" +
                "\r\nALTER TABLE `movie_ratings` ADD PRIMARY KEY IF NOT EXISTS (`id`);";

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Posts movies to database
        public async void PostTopMovies()
        {
            createTable();
            
            List<Movie> movies = await fetchTopMovies();
            movies.ForEach(movie =>
            {
                movie.id = parseString(movie.id);
            });
            // Takes veeery long
            List<string> movieTitles = await fetchMovieTitle(movies);

            List<string> genres = await fetchMovieGenres(movies);
            postGenres(getUniqueGenres(genres));

/*            List<string> movieTitles = new List<string>();
            foreach (Movie movie in movies)
            {
                movieTitles.Add(movie.id + " " + movie.chartRating.ToString());
            }*/

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            string sql = "DELETE FROM `movie_ratings`; ";

            for (int i = 0; i < movies.Count; i++)
            {
                sql += "INSERT INTO movie_ratings VALUES (\"" + movies[i].id + "\", " + movies[i].chartRating + "," +
                    " \"" + movieTitles[i] + "\", \"" + genres[i] + "\"); ";
            }

            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
            await Console.Out.WriteLineAsync("All values was filled and are ready to use with API");
        }

        // Gets movie genres from api
        public async Task<List<string>> fetchMovieGenres(List<Movie> movies)
        {
            List<string> movieGenres = new List<string>();

            var client = new HttpClient();

            for (int i = 0; i < movies.Count; i++)
            {
                string uri = "https://online-movie-database.p.rapidapi.com/title/get-genres?tconst=" + movies[i].id;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                    Headers =
    {
        { "X-RapidAPI-Key", "e2f3a905femsh0fe72e7251ee803p180182jsnea6eb9ca0bf1" },
        { "X-RapidAPI-Host", "online-movie-database.p.rapidapi.com" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    var contentString = await response.Content.ReadAsStringAsync();

                    movieGenres.Add(parseGenres(contentString));
                }
            }
            await Console.Out.WriteLineAsync("Genres fetched successfully");
            return movieGenres;
        }

        // Gets movies title from api by id
        public async Task<List<string>> fetchMovieTitle(List<Movie> movies)
        {
            List<string> movieTitles = new List<string>();

            var client = new HttpClient();

            for (int i = 0; i < movies.Count; i++)
            {
                string uri = "https://online-movie-database.p.rapidapi.com/title/get-details?tconst=" + movies[i].id;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                    Headers =
    {
        { "X-RapidAPI-Key", "e2f3a905femsh0fe72e7251ee803p180182jsnea6eb9ca0bf1" },
        { "X-RapidAPI-Host", "online-movie-database.p.rapidapi.com" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    var contentString = await response.Content.ReadAsStringAsync();

                    Details details = JsonConvert.DeserializeObject<Details>(contentString);
                    movieTitles.Add(details.title);
                }
            }
            await Console.Out.WriteLineAsync("Titles fetched successfully");
            return movieTitles;
        }

        // Gets 250 top movies from api
        public async Task<List<Movie>> fetchTopMovies()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://online-movie-database.p.rapidapi.com/title/get-top-rated-movies"),
                Headers =
                {
                    { "X-RapidAPI-Key", "e2f3a905femsh0fe72e7251ee803p180182jsnea6eb9ca0bf1" },
                    { "X-RapidAPI-Host", "online-movie-database.p.rapidapi.com" },
                },
            };
            List<Movie> movies = new List<Movie>();
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                using var contentStream = await response.Content.ReadAsStreamAsync();

                var moviesIEnum = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<Movie>>(contentStream);
                movies = moviesIEnum.ToList();
            }
            await Console.Out.WriteLineAsync("Movies fetched successfully");
            return movies;
        }
    }
}
