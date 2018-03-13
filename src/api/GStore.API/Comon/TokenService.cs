using GStore.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace GStore.API.Comon
{
    public class TokenService
    {
        public static string Audience { get; } = "GStoreAudience";
        public static string Issuer { get; } = "GStoreIssuer";

        IConfiguration _config;
        string _token_appkey;
        public int _token_expires_mins;

        public TokenService( IConfiguration configuration )
        {
            _config = configuration;

            _token_appkey = _config.GetSection( "GStore" ).GetValue<string>( "token_appkey" );
            _token_expires_mins = _config.GetSection( "GStore" ).GetValue<int>( "token_expires_mins" );
        }

        public SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey( System.Text.Encoding.Default.GetBytes( _token_appkey ) );
        }

        public string GenerateToken( User user )
        {
            var signCredentials = new SigningCredentials( GetSecurityKey(), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature );

            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(
                new GenericIdentity( user.Username, "TokenAuth" ),
                new[] {
                    new Claim( "UserId", user.Id.ToString() ),
                    new Claim( "UserAuthz", user.Authorizations.Aggregate((current, next) => current + "," + next) )
                }
            );

            // set when the token will be expire
            var expiresIn = DateTime.Now + TimeSpan.FromMinutes( _token_expires_mins );

            var securityToken = handler.CreateToken( new SecurityTokenDescriptor {
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = signCredentials,
                Subject = identity,
                Expires = expiresIn
            } );

            return handler.WriteToken( securityToken );
        }

        public bool ReadToken( HttpRequest request, out ClaimsPrincipal principal )
        {
            // check token exists
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
            return new TokenValidationParameters() {
                IssuerSigningKey = GetSecurityKey(),
                ValidAudience = Audience,
                ValidIssuer = Issuer,
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
