using GStore.API.Common;
using GStore.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
