using GStore.API.Comon;
using GStore.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GStore.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected UnitOfWork UnitOfWork { get; private set; }
        protected ILogger Logger { get; private set; }
        protected IConfiguration Config { get; private set; }

        public BaseController( IConfiguration config,
                               ILogger logger,
                               UnitOfWork unitOfWork )
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
            Config = config;
        }
    }
}
