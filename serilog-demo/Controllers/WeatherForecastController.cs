using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace serilog_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger _logger;

        public WeatherForecastController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            try
            {
                _logger.Information("GetWeatherForecast called.");
                var rnd = new Random();

                if (rnd.Next(0,10)%3 == 0)
                {
                    throw new Exception("Simulated error/exception happened.");
                }

                return Ok(
                    Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Internal error happened.");
                return new StatusCodeResult(500);
            }            
        }
    }
}