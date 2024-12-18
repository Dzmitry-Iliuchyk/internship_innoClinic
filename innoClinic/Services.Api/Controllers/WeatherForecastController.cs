using Microsoft.AspNetCore.Mvc;
using Services.Application.Abstractions.Repositories;
using Services.Domain;

namespace Services.Api.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class WeatherForecastController: ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IServiceRepository _repository;

        public WeatherForecastController( ILogger<WeatherForecastController> logger, IServiceRepository repository ) {
            _logger = logger;
            this._repository = repository;
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
