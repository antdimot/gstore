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
        DataContext _context;
            
        IMongoCollection<T> _collection;

        string _collectionName; 

        public Repository( DataContext context )
        {
            _context = context;

            _collectionName = typeof( T ).Name.ToLower();

            _collection = _context.Database.GetCollection<T>( _collectionName );
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
        public T GetSingle( Expression<Func<T, bool>> condition )
        {
            _context.Logger.LogDebug( "REPOSITORY - GetSingle" );

            try
            {
                var query = _collection.Find<T>( condition );

                var result =  query.SingleOrDefault();

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

        public IReadOnlyList<T> GetList( Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> order = null )
        {
            _context.Logger.LogDebug( "REPOSITORY - GetList" );

            try
            {
                IFindFluent<T, T> query;

                if( condition != null )
                {
                    query = _collection.Find<T>( condition );
                }
                else
                {
                    query = _collection.Find<T>( _ => true );
                }

                if( order != null )
                {
                    return query.SortBy<T, T>( order ).ToList();
                }

                return query.ToList();
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "GetList" );

                throw ex;
            }
        }

        public long Count( Expression<Func<T, bool>> condition = null )
        {
            _context.Logger.LogDebug( "REPOSITORY - Count" );

            try
            {
                if( condition == null ) return _collection.Count( _ => true );

                return _collection.Count( condition );
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "Count" );

                throw ex;
            }
        }

        public bool Exists( Expression<Func<T, bool>> condition )
        {
            _context.Logger.LogDebug( "REPOSITORY - Exists" );

            try
            {
                var result = this.Count( condition );

                return result > 0;
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