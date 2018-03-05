using GStore.Core.Domain;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Data
{
    public class UnitOfWork
    {
        DataContext _context;
        IDictionary<Type, object> _repository;

        public Repository<T> Repository<T>() where T : IEntity<ObjectId>
        {
            if( !_repository.Keys.Contains( typeof( T ) ) )
            {
                var obj = new Repository<T>( _context );

                _repository.Add( typeof( T ), obj );
            }

            return (Repository<T>)_repository[typeof( T )];
        }

        public UnitOfWork( DataContext context )
        {
            _repository = new Dictionary<Type, object>();

            _context = context;

            context.UnitOfWork = this;
        }
    }
}
