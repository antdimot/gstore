using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GStore.API.Comon;
using GStore.API.Models;
using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.AspNetCore.Authorization;
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
        public GeoDataController( IConfiguration config, ILogger<GeoDataController> logger, DataContext context ) :
            base( config, logger, context ) { }

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
        public async Task<ObjectResult> GetByLocation( double lon, double lat, double distance, string[] tag = null )
        {
            Logger.LogDebug( "GET-LOCATION[GeoData]" );

            var repository = UnitOfWork.GeoRepository<GeoData>();

            var items = await repository.GetByLocationAsync( lon, lat, distance, tag );
            if( items.Count == 0 )
            {
                return NotFound( new { lon, lat, distance } );
            }

            var result = items.Select( obj => GeoResult.Create( obj ) );

            return Ok( result );
        }

        [HttpPost]
        public async Task<IActionResult> Post( double lon, double lat, string content )
        {
            Logger.LogDebug( "POST[GeoData]" );

            if( TokenService.ReadToken( Request, out ClaimsPrincipal principal ) )
            {
                string uid = principal.Claims.Where( c => c.Type == "UserId" )
                                             .Select( c => c.Value )
                                             .FirstOrDefault();

                if( ObjectId.TryParse( uid, out ObjectId oid ) )
                {
                    var repository = UnitOfWork.Repository<GeoData>();

                    var result = await repository.InsertAsync( new GeoData {
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