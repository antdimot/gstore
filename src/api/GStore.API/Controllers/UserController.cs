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
        public IEnumerable<User> Get()
        {
            var repository = UnitOfWork.Repository<User>();

            var result = repository.GetList();

            return result;
        }
    }
}