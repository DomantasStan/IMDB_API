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

app.UseAuthorization();

app.MapControllers();

app.Run();
