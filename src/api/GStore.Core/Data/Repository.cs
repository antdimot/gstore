using GStore.Core.Domain;
using Microsoft.Extensions.Logging;
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
                _context.Logger.LogError( ex, "Insert" );

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
                _context.Logger.LogError( ex, "Update" );

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
                _context.Logger.LogError( ex, "Delete" );

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
            try
            {
                var query = Set.Where( predicate );

                var result =  query.SingleOrDefault();

                _context.Logger.LogDebug( "REPOSITORY - GetSingle" );

                return result;
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "GetSingle" );

                throw ex;
            }
        }

        public T GetById( ObjectId id )
        {
            return this.GetSingle( o => o.Id == id );
        }

        public IReadOnlyList<T> GetList( Expression<Func<T, bool>> condition = null, Func<T, string> order = null )
        {
            try
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

                var result = query.ToList();

                _context.Logger.LogDebug( "REPOSITORY - GetList" );

                return result;
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "GetList" );

                throw ex;
            }
        }

        public int Count( Expression<Func<T, bool>> predicate = null )
        {
            try
            {
                var result = ( predicate == null ? Set.Count() : Set.Count( predicate ) );

                _context.Logger.LogDebug( "REPOSITORY - Count" );

                return result;
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "Count" );

                throw ex;
            }
        }

        public bool Exists( Expression<Func<T, bool>> predicate )
        {
            try
            {
                var result = Set.Any( predicate );

                _context.Logger.LogDebug( "REPOSITORY - Exists" );

                return result;
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "Exists" );

                throw;
            }
        }
        #endregion
    }
}