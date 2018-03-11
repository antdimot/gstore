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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MongoDB.Bson;
using System.Security.Principal;
using GStore.API.Comon;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace GStore.API.Controllers
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/user" )]
    public class UserController : BaseController
    {
        public UserController( IConfiguration config, ILogger<UserController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork ) { }

        [Authorize]
        [HttpGet]
        public ObjectResult Get()
        {
            Logger.LogDebug( "GET[User]" );

            var repository = UnitOfWork.Repository<User>();

            var users = repository.GetList( u => !u.Deleted )
                                  .Select( u => UserResult.Create( u ) );

            return Ok( users );
        }

        [Authorize]
        [HttpGet( "{id}" )]
        public IActionResult Get( string id )
        {
            Logger.LogDebug( "GET[User]" );

            if( ObjectId.TryParse( id, out ObjectId oid ) )
            {
                var repository = UnitOfWork.Repository<User>();

                var user = repository.GetById( oid );

                if( user == null ) return new NotFoundObjectResult( oid );

                return Ok( UserResult.Create( user ) );
            }

            return BadRequest();
        }

        [HttpPost( "authenticate" )]
        public IActionResult Authenticate( string username, string password )
        {
            var repository = UnitOfWork.Repository<User>();

            var user = repository.GetSingle( u =>   u.Username == username &&
                                                    u.Password == password &&
                                                    !u.Deleted );

            if( user == null ) return Forbid();

            var requestAt = DateTime.Now;
            var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;

            var util = new Utils( Config );

            return Ok( new { accesstoken = util.GenerateToken( user, expiresIn ) } );
        }  
    }
}