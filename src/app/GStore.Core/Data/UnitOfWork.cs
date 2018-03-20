using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace GStore.Core.Data
{
    public class UnitOfWork
    {
        DataContext _context;
        IDictionary<Type, object> _repository;
        IDictionary<Type, object> _geoRepository;

        public Repository<T> Repository<T>() where T : IEntity<ObjectId>
        {
            if( !_repository.Keys.Contains( typeof( T ) ) )
            {
                var obj = new Repository<T>( _context );

                _repository.Add( typeof( T ), obj );
            }

            return (Repository<T>)_repository[typeof( T )];
        }

        public GeoRepository<T> GeoRepository<T>() where T : ILocalizableEntity<ObjectId>
        {
            if( !_geoRepository.Keys.Contains( typeof( T ) ) )
            {
                var obj = new GeoRepository<T>( _context );

                _geoRepository.Add( typeof( T ), obj );
            }

            return (GeoRepository<T>)_geoRepository[typeof( T )];
        }

        public UnitOfWork( DataContext context )
        {
            _repository = new Dictionary<Type, object>();
            _geoRepository = new Dictionary<Type, object>();

            _context = context;

            context.UnitOfWork = this;
        }
    }
}
