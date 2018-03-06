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
    [Produces("application/json")]
    [Route( "api/user" )]
    public class UserController : BaseController
    {
        public UserController( IConfiguration config, ILogger<UserController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork )
        {
        }

        [HttpGet]
        public IEnumerable<UserResult> Get()
        {
            var repository = UnitOfWork.Repository<User>();

            var users = repository.GetList( u => !u.Deleted );

            var result = users.Select(u => new UserResult
            {
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Username = u.Username,
                Email = u.Email
            });

            Logger.LogDebug( "GET - Users" );

            return result;
        }
    }
}