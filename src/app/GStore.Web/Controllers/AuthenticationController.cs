using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using GStore.Core.Data;
using GStore.Core.Domain;
using GStore.Web.Models.Authentication;
using GStore.Web.Models;
using GStore.Core;

namespace GStore.Web.Controllers
{
    [Authorize]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController( IConfiguration config,
                                  ILogger<AuthenticationController> logger,
                                  DataContext dataContext ) : base( config, logger, dataContext ) { }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login( string returnUrl )
        {
            var devUsername = Config.GetSection( "GStore" ).GetValue<string>( "DevUsername" );

            if( string.IsNullOrEmpty( devUsername )  )
                return View( new LoginViewModel { ReturnUrl = returnUrl } ); // normal logon

            // logon with a dev user IS ONLY FOR DEVELOPMENT
            var devPassword = Config.GetSection( "GStore" ).GetValue<string>( "DevPassword" );

            if( await LoginUser( devUsername, devPassword ) )
                return RedirectToAction( "main", "home" );

            return View( new LoginViewModel { ReturnUrl = returnUrl } ); // normal logon
        }

        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login( LoginViewModel model  )
        {
            if( ModelState.IsValid )
            {
                if( await LoginUser( model.Username, model.Password ) )
                    return RedirectToAction( "main", "home" );
            }

            ModelState.AddModelError( "Username", "Invalid credentials" );

            return View( model );
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            LoggedUserInfo.Current = null;

            HttpContext.SignOutAsync();

            return RedirectToAction( "index", "home" );
        }
    
        private async Task<bool> LoginUser( string username, string password )
        {
            // create hashed version of password
            var salt = Config.GetSection( "GStore" ).GetValue<string>( "password_salt" );
            var password_hash = new HashUtility().MakeHash( password, salt );

            var user = await UnitOfWork.Repository<User>()
                                       .GetSingleAsync( u => u.Username == username &&
                                                             u.Password == password_hash &&
                                                             u.Enabled && !u.Deleted );
            if( user != null )
            {
                var claims = new List<Claim> { new Claim( ClaimTypes.Name, user.Username ) };
                var identity = new ClaimsIdentity( claims, CookieAuthenticationDefaults.AuthenticationScheme );
                var principal = new ClaimsPrincipal( identity );

                var loggedUserInfo = new LoggedUserInfo {
                    UserId = user.Id.ToString(),
                    Authorizzations = user.Authorizations,
                    Username = user.Username,
                    DisplayName = $"{user.Firstname} {user.Lastname}",
                };

                LoggedUserInfo.Current = loggedUserInfo; // store user info to session

                await HttpContext.SignInAsync( principal, new AuthenticationProperties { IsPersistent = true } );

                return true;
            }

            return false;
        }
    }
}