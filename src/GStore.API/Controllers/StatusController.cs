using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GStore.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GStore.API.Controllers
{
    [Produces( "application/json" )]
    [ApiVersion( "1" )]
    [Route( "api/v{version:apiVersion}/status" )]
    public class StatusController : BaseController
    {
        public StatusController( IConfiguration config, ILogger<UserController> logger, DataContext context ) :
             base( config, logger, context )
        { }

        [HttpGet( "check" )]
        public IActionResult Check()
        {
            Logger.LogDebug( "GET[Check]" );

            var message = "{ 'message':'GSTore API is up and running!' }";

            return Ok( message );
        }
    }
}