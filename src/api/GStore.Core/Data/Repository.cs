using GStore.Core.Domain;
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
        public IMongoCollection<T> Collection { get { return _collection; } }

        DataContext _context;
            
        IMongoCollection<T> _collection;

        string _collectionName;
        
        public IQueryable<T> Set { get; private set; }

        public Repository( DataContext context )
        {
            _context = context;

            _collectionName = typeof( T ).Name.ToLower();

            _collection = _context.Database.GetCollection<T>( _collectionName );

            Set = _collection.AsQueryable<T>();
        }

        #region WRITE METHODS
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

        public void InitColletion()
        {
            _context.Database.DropCollection( _collectionName );
        }
        #endregion

        #region QUERY METHODS
        public T GetSingle( Expression<Func<T, bool>> predicate )
        {
            var query = Set.Where( predicate );

            return query.SingleOrDefault();
        }

        public T GetById( ObjectId id )
        {
            return this.GetSingle( o => o.Id == id );
        }

        public IReadOnlyList<T> GetList( Expression<Func<T, bool>> condition = null, Func<T, string> order = null )
        {
            var query = this.Set;
                 
            if( condition != null )
            {
                query = query.Where( condition );
            }

            if( order != null )
            {
                return query.OrderBy( order ).ToList();
            }

            return query.ToList();
        }

        public int Count( Expression<Func<T, bool>> predicate = null )
        {
            return ( predicate == null ? Set.Count() : Set.Count( predicate ) );
        }

        public bool Exists( Expression<Func<T, bool>> predicate )
        {
            return Set.Any( predicate );
        }
        #endregion
    }
}