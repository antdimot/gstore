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

namespace GStore.API.Controllers
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/user" )]
    public class UserController : BaseController
    {
        public UserController( IConfiguration config, ILogger<UserController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork ) { }

        [HttpGet]
        public ObjectResult Get()
        {
            Logger.LogDebug( "GET[User]" );

            var repository = UnitOfWork.Repository<User>();

            var users = repository.GetList( u => !u.Deleted )
                                  .Select( u => UserResult.Create( u ) );

            return Ok( users );
        }

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

            return Ok( new { accesstoken = GenerateToken( user, expiresIn ) } );
        }

        private string GenerateToken( User user, DateTime expires )
        {
            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(
                new GenericIdentity( user.Username, "TokenAuth" ),
                new[] { new Claim( "ID", user.Id.ToString() ) }
            );

            var securityToken = handler.CreateToken( new SecurityTokenDescriptor {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            } );

            return handler.WriteToken( securityToken );
        }
    }
}