﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GStore.Web.Models;
using GStore.Core.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GStore.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;

namespace GStore.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController( IConfiguration config, ILogger<HomeController> logger, DataContext dataContext ) :
            base( config, logger, dataContext ) { }

        public IActionResult Index()
        {
            return View( new IndexViewModel() );
        }

        [Authorize]
        public IActionResult Main()
        {
            return View( new MainViewModel() );
        }
    }
}
