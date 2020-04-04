using GStore.API.Common;
using GStore.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Linq;
using System.Security.Claims;

namespace GStore.API.Controllers
{
    public abstract class BaseController : Controller
    {
        SecurityService _tokenService;
        UnitOfWork _unitOfWork;

        protected ILogger Logger { get; private set; }
        protected IConfiguration Config { get; private set; }
        protected DataContext DataContext { get; private set; }

        protected UnitOfWork UnitOfWork
        {
            get
            {
                if( _unitOfWork == null ) _unitOfWork = new UnitOfWork( DataContext );

                return _unitOfWork;
            } 
        }

        protected SecurityService SecurityService
        {
            get
            {
                if( _tokenService == null ) _tokenService = new SecurityService( Config );

                return _tokenService;
            }
        }

        protected ObjectId? GetUserId()
        {
            if( SecurityService.ReadToken( Request, out ClaimsPrincipal principal ) )
            {

                string uid = principal.Claims.Where( c => c.Type == "UserId" )
                                             .Select( c => c.Value )
                                             .Single();

                if( ObjectId.TryParse( uid, out ObjectId oid ) )
                {
                    return oid;
                }
            }

            return null;
        }

        public BaseController( IConfiguration config,
                               ILogger logger,
                               DataContext dataContext )
        {
            Logger = logger;
            Config = config;
            DataContext = dataContext;
        }
    }
}
