using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using simple_crud.ApplicationConfiguration;

namespace simple_crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IApplicationConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IApplicationConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("test")]
        public string Get2()
        {
            try
            {
                _logger.LogInformation(_configuration.ConnectionString);
                _logger.LogWarning(_configuration.ConnectionString);
                _logger.LogError(_configuration.ConnectionString);
                _logger.LogCritical(_configuration.ConnectionString);


                using var connection = new SqlConnection(_configuration.ConnectionString);
                using var command = new SqlCommand("SELECT TOP 1 Name FROM Test", connection);
                command.Connection.Open();

                var result = command.ExecuteScalar();
                return result.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
