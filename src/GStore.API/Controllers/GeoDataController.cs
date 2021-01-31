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

        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( string id )
        {
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

        [HttpPost( "delete" )]
        public async Task<IActionResult> Post( string id )
        {
            if( !ObjectId.TryParse( id, out ObjectId oid ) )
                return BadRequest();

            var uid = GetUserId();

            var result = await UnitOfWork.Repository<GeoData>()
                                         .Delete( o => o.Id == oid && o.UserId == uid );

            if( result > 0 )
                return Ok();
                    
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post( string lon, string lat, string name, string content )
        {
            if( lon == null || lat == null|| name == null || content == null )
                return BadRequest();

            lon = lon.Replace( '.', ',' );
            lat = lat.Replace( '.', ',' );

            if( !double.TryParse( lat, out double latitude ) )
                return BadRequest( "invalid latitude" );
            if( !double.TryParse( lon, out double longitude ) )
                return BadRequest("invalid longitude");

            var oid = GetUserId();

            if( !oid.HasValue ) return BadRequest();

            var repository = UnitOfWork.Repository<GeoData>();

            var result = await repository.InsertAsync( new GeoData
            {
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>( new GeoJson2DGeographicCoordinates( longitude, latitude ) ),
                Content = content,
                ContentType = Utils.ContentType.Text,
                UserId = oid.Value,
                Name = name
            } );

            return Ok();
        }

        [HttpGet( "list" )]
        public async Task<IActionResult> List()
        {
            var oid = GetUserId();

            if( oid.HasValue )
            {
                var items = await UnitOfWork.Repository<GeoData>()
                                             .GetListAsync( o => o.UserId == oid.Value );

                if( items.Count() == 0 )
                {
                    return NotFound();
                }

                var result = items.Select( obj => GeoResult.Create( obj ) );

                return Ok( result );
            }          

            return BadRequest();
        }

        [HttpGet( "all" )]
        [Authorize( Policy = "AdminApi" )]
        public async Task<IActionResult> All()
        {
            var items = await UnitOfWork.Repository<GeoData>()
                                        .GetListAsync();

            return Ok( items.Select( obj => GeoResult.Create( obj ) ) );
        }
    }
}