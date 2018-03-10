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

namespace GStore.API.Controllers
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/user" )]
    public class UserController : BaseController
    {
        public UserController( IConfiguration config, ILogger<UserController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork ) { }

        //[HttpGet]
        //public ObjectResult Get()
        //{
        //    Logger.LogDebug( "GET[User]" );

        //    var repository = UnitOfWork.Repository<User>();

        //    var users = repository.GetList( u => !u.Deleted )
        //                          .Select( u => UserResult.Create( u ) );

        //    return Ok( users );
        //}

        [HttpGet]
        public ObjectResult Get( string id  )
        {
            Logger.LogDebug( "GET[User]" );

            var repository = UnitOfWork.Repository<User>();

            var user = repository.GetById( ObjectId.Parse( id ) );

            if( user == null ) return new NotFoundObjectResult( id );

            return Ok( UserResult.Create( user ) );
        }

        [HttpPost]
        public IActionResult Authenticate( string username, string password )
        {
            var repository = UnitOfWork.Repository<User>();

            var user = repository.GetSingle( u =>   u.Username == username &&
                                                    u.Password == password &&
                                                    !u.Deleted );

            if( user != null )
            {
                return Ok( new { accesstoken = createToken( username ) } );
            }
            else
            {
                return Forbid();
            }
        }

        private string createToken( string username )
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;

            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays( 7 );

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity( new[]
            {
                new Claim( ClaimTypes.Name, username )
            } );

            // todo: move secret to safe place 
            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey( System.Text.Encoding.Default.GetBytes( sec ) );
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials( securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature );

            //create the jwt
            var token = (JwtSecurityToken) tokenHandler.CreateJwtSecurityToken( 
                                                issuer: "http://localhost:50191",
                                                audience: "http://localhost:50191",
                                                subject: claimsIdentity,
                                                notBefore: issuedAt,
                                                expires: expires,
                                                signingCredentials: signingCredentials );

            var tokenString = tokenHandler.WriteToken( token );

            return tokenString;
        }
    }
}