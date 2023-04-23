using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IMDB_API.Scheduler
{
    public class SchedulerService :IHostedService, IDisposable
    {
        private int executionCount = 0;

        private System.Threading.Timer _timerNotification;
        public IConfiguration _iconfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public SchedulerService(IServiceScopeFactory serviceScopeFactory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration iconfiguration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _env = env;
            _iconfiguration = iconfiguration;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            // Here you can specifie time in which api fetches data and updates database
            _timerNotification = new Timer(RunJob, null, TimeSpan.Zero, TimeSpan.FromMinutes(20));//TimeSpan.FromDays(7));

            return Task.CompletedTask;
        }

        public void RunJob(object state)
        {
            try
            {
                StartUp start = new StartUp(_iconfiguration.GetConnectionString("MovieCS"));
                start.PostTopMovies();
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Interlocked.Increment(ref executionCount);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timerNotification?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerNotification?.Dispose();
        }
    }
}
