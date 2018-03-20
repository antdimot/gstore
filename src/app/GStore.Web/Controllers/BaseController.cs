using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GStore.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
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