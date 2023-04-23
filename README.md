# IMDB_API

## What does this API do ?
This api retrieves 250 top rated movies from IMDB using RAPID API. Saves movie data (id, rating, title, genres) and unique genres of the movies to specified database. Then using endpoints user can get data of top 250 movies by genre ordered by rating from highest to lowest.

## How to use this api ?
### Prerequisites
This project was developed and tested using Visual Studio Community 2022. Also user needs a database which name will be specified in the connectionString attribute.

### Setting up project
Clone project to your machine and open it with 'IMDB_API.sln' file in projects directory. Once project is opened in Visual Studio user need to change connectionString attribute in appsettings.json located in main projects folder. User should only change value of 'MovieCS' attribute. Default value look like this: 'server=localhost;database=movies;user=root;password=' where server is your database server name, database is your database name, user is your database username, password is your database password. You do not need to create any tables in your database. Tables 'genres' and 'movie_ratings' will be created automatically.

### Starting an api
After project is set up user can start a project by clicking 'Start' button in Visual Studio. When api is started user must wait untill for message 'All values was filled and are ready to use with API' in Visual Studio console. This should take about 10 - 15 minutes. This procedure takes that long because this api makes over 500 calls to another api to gather the data. After the message user can  start using the api.

### Using the api
IMDB API has two endpoints:

GetUniqueGenres: takes no arguuments and returns a list of unique genres of the top 250 IMDB movies. This data should help user to pick wanted genre for the movies.
Request example: http://localhost:5029/api/Movie/GetUniqueGenres

GetMoviesByGenre: takes one argument genre. Genre can be picked from a list of unique genres described above. User can filter movies by one genre at the time. This endpoint returns a list of movies which fit to a picked genre. Movie information: id, rating, title, genres. Results are sorted by rating from highest to lowest.
Request example: http://localhost:5029/api/Movie/GetMoviesByGenre/Comedy
