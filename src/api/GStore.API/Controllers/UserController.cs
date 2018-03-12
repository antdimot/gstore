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
        public UserController( IConfiguration config, ILogger<UserController> logger, DataContext context ) :
            base( config, logger, context ) { }

        [Authorize( Policy = "AdminApi" )]
        [HttpGet("list")]
        public async Task<ObjectResult> List()
        {
            Logger.LogDebug( "GET[User]" );

            var repository = UnitOfWork.Repository<User>();

            var users = await repository.GetListAsync( u => !u.Deleted );

            var result = users.Select( u => UserResult.Create( u ) );

            return Ok( result );
        }

        [Authorize( Policy = "AdminApi" )]
        [Authorize]
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( string id )
        {
            Logger.LogDebug( "GET[User]" );

            if( ObjectId.TryParse( id, out ObjectId oid ) )
            {
                var repository = UnitOfWork.Repository<User>();

                var user = await repository.GetByIdAsync( oid );

                if( user == null ) return new NotFoundObjectResult( oid );

                return Ok( UserResult.Create( user ) );
            }

            return BadRequest();
        }

        [HttpPost( "authenticate" )]
        public async Task<IActionResult> Authenticate( string username, string password )
        {
            var repository = UnitOfWork.Repository<User>();

            var user = await repository.GetSingleAsync( u =>    u.Username == username &&
                                                                u.Password == password &&
                                                                !u.Deleted );

            if( user == null ) return Forbid();

            var requestAt = DateTime.Now;
            var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;

            return Ok( new {
                token = TokenService.GenerateToken( user, expiresIn )
            } );
        }  
    }
}