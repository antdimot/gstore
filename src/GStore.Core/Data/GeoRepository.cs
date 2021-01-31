using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GStore.Core.Data
{
    public class GeoRepository<T> : Repository<T> where T : ILocalizableEntity<ObjectId>
    {
        readonly Func<double,double> radiusOf = x => x / 6378.1; // calc radius by km

        public GeoRepository( DataContext context ) : base( context ) { }

        public async Task<IReadOnlyList<T>> GetByLocationAsync( ObjectId oid, double longitude, double latitude, double distance, string[] tags = null )
        {
            Context.Logger.LogDebug( "REPOSITORY - GetByLocation" );

            try
            {
                var filterBuilder = Builders<T>.Filter;

                // set userid filter
                var filters = new List<FilterDefinition<T>> { filterBuilder.Where( o => o.UserId == oid ) };

                // set geo filter
                filters.Add( filterBuilder.GeoWithinCenterSphere(
                                            o => o.Location,
                                            longitude, latitude, radiusOf( distance ) ));

                // set tags filter
                if( tags != null && tags.Length > 0 )
                {
                    filters.Add( filterBuilder.AnyIn<string>( o => o.Tags, tags ) );
                }

                var query = await Context.Database.GetCollection<T>( CollectionName )
                                                   .Find<T>( filterBuilder.And( filters ) )
                                                   .Limit( Limit )
                                                   .ToCursorAsync();

                return await query.ToListAsync();
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "GetByLocation" );

                throw;
            }
        }
    }
}
