using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GStore.Core.Data
{
    public class Repository<T> where T : IEntity<ObjectId>
    {
        DataContext _context;

        IMongoCollection<T> _collection;

        public IMongoCollection<T> Collection { get { return _collection; } }

        public Repository( DataContext context )
        {
            _context = context;

            _collection = _context.GetDatabase().GetCollection<T>( typeof( T ).Name.ToLower() );
        }

        private IQueryable<T> CreateSet()
        {
            return _collection.AsQueryable<T>();
        }

        public T Insert( T instance )
        {
            try
            {
                instance.Id = ObjectId.GenerateNewId();

                _collection.InsertOne( instance );

                return instance;
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }

        public void Update( T instance )
        {
            try
            {
                Expression<Func<T, bool>> filter = x => x.Id == instance.Id;

                var update = new ObjectUpdateDefinition<T>( instance );

                _collection.UpdateOne<T>( filter, update );
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }

        public void Delete( ObjectId id, bool logical = true )
        {
            try
            {
                Expression<Func<T, bool>> filter = x => x.Id == id;

                if( logical )
                {
                    var update = new JsonUpdateDefinition<T>( "deleted:'false'" );

                    _collection.UpdateOne<T>( filter, update );
                }
                else
                {
                    _collection.DeleteOne<T>( filter );
                }
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }

        public T GetById( ObjectId id )
        {
            return this.Single( o => o.Id == id );
        }

        public T Single( Expression<Func<T, bool>> predicate = null )
        {
            var set = CreateSet();

            var query = ( predicate == null ? set : set.Where( predicate ) );

            return query.SingleOrDefault();
        }

        public IReadOnlyList<T> List( Expression<Func<T, bool>> condition = null, Func<T, string> order = null )
        {
            var set = CreateSet();

            if( condition != null )
            {
                set = set.Where( condition );
            }

            if( order != null )
            {
                return set.OrderBy( order ).ToList();
            }

            return set.ToList();
        }

        public int Count( Expression<Func<T, bool>> predicate = null )
        {
            var set = CreateSet();

            return ( predicate == null ? set.Count() : set.Count( predicate ) );
        }

        public bool Exists( Expression<Func<T, bool>> predicate )
        {
            var set = CreateSet();

            return set.Any( predicate );
        }
    }  
}
