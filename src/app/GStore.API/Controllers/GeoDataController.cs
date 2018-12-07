using GStore.API.Common;
using GStore.API.Models;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GStore.API.Controllers
{
    [Produces( "application/json" )]
    [ApiVersion( "1" )]
    [Route( "api/v{version:apiVersion}/geodata" )]
    [Authorize]
    public class GeoDataController : BaseController
    {
        public GeoDataController( IConfiguration config, ILogger<GeoDataController> logger, DataContext context ) :
            base( config, logger, context )
        { }

        [Authorize( Policy = "AdminApi" )]
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( string id )
        {
            Logger.LogDebug( "GET[GeoData]" );

            if( ObjectId.TryParse( id, out ObjectId oid ) )
            {
                var item = await UnitOfWork.Repository<GeoData>()
                                           .GetByIdAsync( oid );

                if( item == null ) return NotFound( id );

                return Ok( GeoResult.Create( item ) );
            }

            return BadRequest();
        }

        [HttpGet( "location" )]
        public async Task<IActionResult> GetByLocation( double lon, double lat, double distance, string[] tag = null )
        {
            Logger.LogDebug( "GET-LOCATION[GeoData]" );

            var oid = GetUserId();

            if( !oid.HasValue ) return BadRequest();
        
            var repository = UnitOfWork.GeoRepository<GeoData>();

            var items = await repository.GetByLocationAsync( oid.Value, lon, lat, distance, tag );
            if( items.Count() == 0 )
            {
                return NotFound();
            }

            var result = items.Select( obj => GeoResult.Create( obj ) );

            return Ok( result );
        }

        [HttpPost]
        public async Task<IActionResult> Post( double lon, double lat, string content )
        {
            Logger.LogDebug( "POST[GeoData]" );

            var oid = GetUserId();

            if( !oid.HasValue ) return BadRequest();

            var repository = UnitOfWork.Repository<GeoData>();

            var result = await repository.InsertAsync( new GeoData
            {
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>( new GeoJson2DGeographicCoordinates( lon, lat ) ),
                Content = content,
                ContentType = Utils.ContentType.Text,
                UserId = oid.Value
            } );

            return Ok();
        }
    }
}