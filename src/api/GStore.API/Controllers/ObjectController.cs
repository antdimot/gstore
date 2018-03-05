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

namespace GStore.API.Controllers
{
    [Produces("application/json")]
    [Route( "api/object" )]
    public class ObjectController : BaseController
    {
        public ObjectController( IConfiguration config, ILogger<ObjectController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork )
        {
        }

        [HttpGet]
        public IEnumerable<GeoObject> Get()
        {
            var repository = UnitOfWork.Repository<GeoObject>();

            var result = repository.GetList();

            return result;
        }

        [HttpPost]
        public void Post( double lat, double lon )
        {
            var repository = UnitOfWork.Repository<GeoObject>();

            var result = repository.Insert( new GeoObject {
                Latitude = lat,
                Longitude = lon,
                ObjectData = ""
            } );
        }
    }
}