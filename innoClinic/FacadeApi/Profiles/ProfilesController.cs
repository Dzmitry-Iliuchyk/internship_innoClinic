using FacadeApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FacadeApi.Profiles {
    [ApiController]
    [Route( "[controller]" )]
    public class ProfilesController: ControllerBase {


        private readonly ILogger<WeatherForecastController> _logger;

        public ProfilesController( ILogger<WeatherForecastController> logger ) {
            _logger = logger;
        }

        //[HttpGet( "[action]" )]
        //public async Task<IResult> Get() {

        //}
    }
}
