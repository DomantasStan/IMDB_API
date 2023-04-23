# IMDB_API

## What does this API do ?
This api retrieves 250 top rated movies from IMDB using RAPID API. Saves movie data (id, rating, title, genres) and unique genres of the movies to specified database. Then using endpoints user can get data of top 250 movies by genre ordered by rating from highest to lowest.

## How to use this api ?
### Prerequisites
This project was made and tested with Visual Studio Community 2022. Also user needs a database which name will be specified in the connectionString attribute.

### Setting up project
Once project is opened in Visual Studio user need to change connectionString attribute in appsettings.json located in main projects folder. You should only change value of 'MovieCS' attribute. Default value look like this: 'server=localhost;database=movies;user=root;password=' where server is your database server name, database is your database name, user is your database username, password is your database password. You do not need to create any tables in your database. Tables 'genres' and 'movie_ratings' will be created automatically.

### Starting an api
After project is set up user can start a project by clicking 'Start' button in Visual Studio. 
