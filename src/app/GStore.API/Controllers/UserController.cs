using GStore.API.Common;
using GStore.API.Models;
using GStore.Core;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GStore.API.Controllers
{
    [Produces( "application/json" )]
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/user" )]
    public class UserController : BaseController
    {
        public UserController( IConfiguration config, ILogger<UserController> logger, DataContext context ) :
            base( config, logger, context )
        { }

        /// <summary>
        /// Provides access token by authentication of credentials
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        [HttpPost( "authenticate" )]
        public async Task<IActionResult> Authenticate( string username, string password )
        {
            Logger.LogDebug( "POST[Authenticate]" );

            if( string.IsNullOrEmpty( username ) || string.IsNullOrEmpty( password ) ) return BadRequest();

            // create hashed version of password
            var salt = Config.GetSection( "GStore" ).GetValue<string>( "password_salt" );
            var password_hash = new HashUtility().MakeHash( password, salt );

            var user = await UnitOfWork.Repository<User>()
                                       .GetSingleAsync( u => u.Username == username &&
                                                             u.Password == password_hash &&
                                                             u.Enabled &&
                                                             !u.Deleted );

            if( user == null ) return Forbid();

            return Ok( new
            {
                token = SecurityService.GenerateToken( user )
            } );
        }

        [Authorize( Policy = "AdminApi" )]
        [HttpGet( "list" )]
        public async Task<IActionResult> List()
        {
            Logger.LogDebug( "GET[List]" );

            var users = await UnitOfWork.Repository<User>().GetListAsync( u => !u.Deleted );

            var result = users.Select( u => UserResult.Create( u ) );

            return Ok( result );
        }

        [Authorize( Policy = "AdminApi" )]
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( string id )
        {
            Logger.LogDebug( "GET[User]" );

            var oid = GetUserId();

            if( !oid.HasValue ) return BadRequest();

            var user = await UnitOfWork.Repository<User>().GetByIdAsync( oid.Value );

            if( user == null ) return new NotFoundObjectResult( oid );

            return Ok( UserResult.Create( user ) );
        }

        [Authorize]
        [HttpGet( "info" )]
        public async Task<IActionResult> Info()
        {
            Logger.LogDebug( "GET[Info]" );

            var oid = GetUserId();

            if( !oid.HasValue ) return BadRequest();

            var user = await UnitOfWork.Repository<User>().GetByIdAsync( oid.Value );

            if( user == null ) return new NotFoundObjectResult( oid );

            return Ok( UserResult.Create( user ) );
        }
    }
}