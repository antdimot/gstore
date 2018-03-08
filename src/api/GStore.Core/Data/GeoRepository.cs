using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Data
{
    public class GeoRepository<T> : Repository<T> where T : ILocalizableEntity<ObjectId>
    {
        public GeoRepository( DataContext context ) : base( context )
        {
        }

        public IReadOnlyList<T> GetByLocation( double longitude, double latitude, double radius )
        {
            _context.Logger.LogDebug( "REPOSITORY - GetByLocation" );

            try
            {
                var filterBuilder = new FilterDefinitionBuilder<T>();

                var filter = filterBuilder.GeoWithinCenterSphere(
                                                o => o.Location,
                                                longitude, latitude, radius );

                var query = _context.Database.GetCollection<T>( _collectionName )
                                             .Find<T>( filter );

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
