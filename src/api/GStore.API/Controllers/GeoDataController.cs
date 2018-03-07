using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GStore.Core;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GStore.API.Controllers
{
    //[Produces("application/json")]
    [Route( "api/geodata" )]
    public class GeoDataController : BaseController
    {
        public GeoDataController( IConfiguration config, ILogger<GeoDataController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork )
        { }

        [HttpGet]
        public ObjectResult Get( string id )
        {
            Logger.LogDebug( "GET[GeoData]" );

            var repository = UnitOfWork.Repository<GeoData>();

            var result = repository.GetById( ObjectId.Parse( id ) );

            if( result == null )
            {
                return NotFound( id );
            }

            return Ok( result.Content );
        }

        [HttpPost]
        public IActionResult Post( string uid, double lat, double lon, string content )
        {
            Logger.LogDebug( "POST[GeoData]" );

            var repository = UnitOfWork.Repository<GeoData>();

            var result = repository.Insert( new GeoData {
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates> (new GeoJson2DGeographicCoordinates( lon, lat) ),
                Content = content,
                ContentType = Util.ContentType.Text,
                UserId = ObjectId.Parse( uid )
            } );

            return Ok();
        }
    }
}