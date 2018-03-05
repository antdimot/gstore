using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GStore.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BaseController
    {
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var _connectionString = "mongodb://localhost:27017";
            var _dbname = "demo";

            var context = new DataContext( _connectionString, _dbname );

            var repository = new Repository<User>( context );

            var result = repository.GetList();

            return result;
        }
    }
}