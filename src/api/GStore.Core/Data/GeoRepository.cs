using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GStore.Core.Data
{
    public class GeoRepository<T> : Repository<T> where T : ILocalizableEntity<ObjectId>
    {
        public GeoRepository( DataContext context ) : base( context ) { }

        public async Task<IReadOnlyList<T>> GetByLocationAsync( double longitude, double latitude, double distance, string[] tags = null )
        {
            _context.Logger.LogDebug( "REPOSITORY - GetByLocation" );

            try
            {
                var radius = distance / 6378.1; // calc radius by km

                var filterBuilder = Builders<T>.Filter;

                var filters = new List<FilterDefinition<T>> {
                        filterBuilder.GeoWithinCenterSphere(
                                                o => o.Location,
                                                longitude, latitude, radius )
                };

                if( tags != null && tags.Length > 0 )
                {
                    filters.Add( filterBuilder.AnyIn<string>( o => o.Tags, tags ) );
                }

                var query = await _context.Database.GetCollection<T>( _collectionName )
                                                   .FindAsync<T>( filterBuilder.And( filters ) );

                return query.ToList();
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "GetByLocation" );

                throw ex;
            }
        }
    }
}
