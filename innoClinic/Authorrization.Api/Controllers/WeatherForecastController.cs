using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorrization.Api.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class WeatherForecastController: ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController( ILogger<WeatherForecastController> logger ) {
            _logger = logger;
        }

        [HttpGet( "[action]" )]
        public IEnumerable<WeatherForecast> GetWeatherForecast() {
            return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast {
                Date = DateOnly.FromDateTime( DateTime.Now.AddDays( index ) ),
                TemperatureC = Random.Shared.Next( -20, 55 ),
                Summary = Summaries[ Random.Shared.Next( Summaries.Length ) ]
            } )
            .ToArray();
        }
        [Authorize]
        [HttpGet( "[action]" )]
        public IEnumerable<WeatherForecast> GetWeatherForecastAuthorized() {
            return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast {
                Date = DateOnly.FromDateTime( DateTime.Now.AddDays( index ) ),
                TemperatureC = Random.Shared.Next( -20, 55 ),
                Summary = Summaries[ Random.Shared.Next( Summaries.Length ) ]
            } )
            .ToArray();
        }
    }
}
