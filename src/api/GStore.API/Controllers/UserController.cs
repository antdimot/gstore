using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GStore.API.Models;

namespace GStore.API.Controllers
{
    //[Produces("application/json")]
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/user" )]
    public class UserController : BaseController
    {
        public UserController( IConfiguration config, ILogger<UserController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork ) { }

        [HttpGet]
        public ObjectResult Get()
        {
            var repository = UnitOfWork.Repository<User>();

            var users = repository.GetList( u => !u.Deleted );

            var result = users.Select(u => new UserResult
            {
                Id = u.Id.ToString(),  
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Username = u.Username,
                Email = u.Email
            });

            Logger.LogDebug( "GET[User]" );

            return Ok( result );
        }
    }
}