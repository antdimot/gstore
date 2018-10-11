using System.Collections.Generic;
using GStore.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GStore.API.Controllers
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class DefaultController : BaseController
    {
        public DefaultController( IConfiguration config, ILogger<DefaultController> logger, DataContext context ) :
            base( config, logger, context ) { }

        [HttpGet]
        public string Get()
        {
            Logger.LogDebug( "GET[Default]" );

            return "GStore API service up!";
        }
    }
}
