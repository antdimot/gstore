using GStore.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GStore.API.Comon
{
    public class TokenService
    {
        IConfiguration _config;

        public TokenService( IConfiguration configuration )
        {
            _config = configuration;
        }

        public string GenerateToken( User user, DateTime expires )
        {
            var appSecretKey = _config.GetSection( "GStore" ).GetValue<string>( "appkey" );
            var signCredentials = new SigningCredentials( TokenAuthOption.CreateSecurityKey( appSecretKey ), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature );

            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(
                new GenericIdentity( user.Username, "TokenAuth" ),
                new[] {
                    new Claim( "UserId", user.Id.ToString() ),
                    new Claim( "UserRoles", user.Roles.Aggregate((current, next) => current + "," + next) )
                }
            );

            var securityToken = handler.CreateToken( new SecurityTokenDescriptor {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = signCredentials,
                Subject = identity,
                Expires = expires
            } );

            return handler.WriteToken( securityToken );
        }

        public bool ReadToken( HttpRequest request, out ClaimsPrincipal principal )
        {
            if( !request.Headers.TryGetValue( "Authorization", out StringValues authzHeaders ) )
            {
                principal = null;

                return false;
            }

            var bearerToken = authzHeaders.ElementAt( 0 );
            var token = bearerToken.StartsWith( "Bearer " ) ? bearerToken.Substring( 7 ) : bearerToken;

            var handler = new JwtSecurityTokenHandler();

            principal = handler.ValidateToken( token, CreateValidationParams(), out SecurityToken validatedToken );

            return true;
        }

        public TokenValidationParameters CreateValidationParams()
        {
            var appSecretKey = _config.GetSection( "GStore" ).GetValue<string>( "appkey" );

            return new TokenValidationParameters() {
                IssuerSigningKey = TokenAuthOption.CreateSecurityKey( appSecretKey ),
                ValidAudience = TokenAuthOption.Audience,
                ValidIssuer = TokenAuthOption.Issuer,
                // When receiving a token, check that we've signed it.
                ValidateIssuerSigningKey = true,
                // When receiving a token, check that it is still valid.
                ValidateLifetime = true,
                // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                // machines which should have synchronised time, this can be set to zero. and default value will be 5minutes
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
