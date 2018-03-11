using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GStore.API.Comon;
using GStore.API.Models;
using GStore.Core;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GStore.API.Controllers
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/geodata" )]
    [Authorize]
    public class GeoDataController : BaseController
    {
        public GeoDataController( IConfiguration config, ILogger<GeoDataController> logger, UnitOfWork unitOfWork ) : base( config, logger, unitOfWork ) { }

        [HttpGet( "{id}" )]
        public IActionResult Get( string id )
        {
            Logger.LogDebug( "GET[GeoData]" );

            if( ObjectId.TryParse( id, out ObjectId oid ) )
            {
                var item = UnitOfWork.Repository<GeoData>()
                                     .GetById( oid );

                if( item == null ) return NotFound( id );

                return Ok( new GeoResult {
                    Id = item.Id.ToString(),
                    Longitude = item.Location.Coordinates.Longitude,
                    Latitude = item.Location.Coordinates.Latitude,
                    Content = item.Content,
                    ContentType = item.ContentType
                } );
            }

            return BadRequest();
        }

        [HttpGet( "content/{id}" )]
        public IActionResult GetContent( string id )
        {
            Logger.LogDebug( "GET-CONTENT[GeoData]" );

            if( ObjectId.TryParse( id, out ObjectId oid ) )
            {
                var item = UnitOfWork.Repository<GeoData>()
                                     .GetById( oid );

                if( item == null ) return NotFound( id );

                return Ok( item.Content );
            }

            return BadRequest();
        }

        [HttpGet( "location" )]
        public ObjectResult GetByLocation( double lon, double lat, double distance )
        {
            Logger.LogDebug( "GET-LOCATION[GeoData]" );

            var repository = UnitOfWork.GeoRepository<GeoData>();

            var items = repository.GetByLocation( lon, lat, distance );
            if( items.Count == 0 )
            {
                return NotFound( new { lon, lat, distance } );
            }

            var result = items.Select( obj => new GeoResult {
                Id = obj.Id.ToString(),
                Longitude = obj.Location.Coordinates.Longitude,
                Latitude = obj.Location.Coordinates.Latitude,
                Content = obj.Content,
                ContentType = obj.ContentType
            } );

            return Ok( result );
        }

        [HttpPost]
        public IActionResult Post( double lon, double lat, string content )
        {
            Logger.LogDebug( "POST[GeoData]" );

            if( Utils.ReadToken( Request, out ClaimsPrincipal principal ) )
            {
                string uid = principal.Claims.Where( c => c.Type == "ID" )
                                             .Select( c => c.Value )
                                             .FirstOrDefault() ;

                if( ObjectId.TryParse( uid, out ObjectId oid ) )
                {
                    var repository = UnitOfWork.Repository<GeoData>();

                    var result = repository.Insert( new GeoData {
                        Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>( new GeoJson2DGeographicCoordinates( lon, lat ) ),
                        Content = content,
                        ContentType = Utils.ContentType.Text,
                        UserId = ObjectId.Parse( uid )
                    } );

                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}