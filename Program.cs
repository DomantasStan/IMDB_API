using IMDB_API.Controllers;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using IMDB_API;
using IMDB_API.Scheduler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(builder.Configuration.GetConnectionString("MovieCS")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//This line activates scheduler which will start data fetching and posting to database every week
builder.Services.AddHostedService<SchedulerService>();

var connection = new MySqlConnection(builder.Configuration.GetConnectionString("MovieCS"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//MovieController movieController = new MovieController(builder.Configuration.GetConnectionString("MovieCS"));
//StartUp start = new StartUp(builder.Configuration.GetConnectionString("MovieCS"));

// To start the API we need to have data ready for user to use
// This method takes data from an api and posts it to database
// this should be used only the first time launching the api or when wanting to update the data
start.PostTopMovies();

app.UseAuthorization();

app.MapControllers();

app.Run();
