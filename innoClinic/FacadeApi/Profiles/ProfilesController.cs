using FacadeApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FacadeApi.Profiles {
    [ApiController]
    [Route( "[controller]" )]
    public class ProfilesController: ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public ProfilesController( ILogger<WeatherForecastController> logger ) {
            _logger = logger;
        }

        [HttpGet( Name = "GetWeatherForecast" )]
        public IEnumerable<WeatherForecast> Get() {
            return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast {
                Date = DateOnly.FromDateTime( DateTime.Now.AddDays( index ) ),
                TemperatureC = Random.Shared.Next( -20, 55 ),
                Summary = Summaries[ Random.Shared.Next( Summaries.Length ) ]
            } )
            .ToArray();
        }
    }
}
