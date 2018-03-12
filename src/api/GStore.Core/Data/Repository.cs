using GStore.Core.Domain;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GStore.Core.Data
{
    public class Repository<T> where T : IEntity<ObjectId>
    {
        protected DataContext _context;

        protected IMongoCollection<T> _collection;

        protected string _collectionName; 

        public Repository( DataContext context )
        {
            _context = context;

            _collectionName = typeof( T ).Name.ToLower();

            _collection = _context.Database.GetCollection<T>( _collectionName );
        }

        #region WRITE METHODS
        public async Task<T> InsertAsync( T instance )
        {
            try
            {
                instance.Id = ObjectId.GenerateNewId();

                await _collection.InsertOneAsync( instance );

                return instance;
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "Insert" );

                throw ex;
            }
        }

        public async void Update( T instance )
        {
            try
            {
                Expression<Func<T, bool>> filter = x => x.Id == instance.Id;

                var update = new ObjectUpdateDefinition<T>( instance );

                await _collection.UpdateOneAsync<T>( filter, update );
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "Update" );

                throw ex;
            }
        }

        public async void Delete( ObjectId id, bool logical = true )
        {
            try
            {
                Expression<Func<T, bool>> filter = x => x.Id == id;

                if( logical )
                {
                    var update = new JsonUpdateDefinition<T>( "deleted:'false'" );

                    await _collection.UpdateOneAsync<T>( filter, update );
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
        public async Task<T> GetSingleAsync( Expression<Func<T, bool>> condition )
        {
            _context.Logger.LogDebug( "REPOSITORY - GetSingle" );

            try
            {
                var query = await _collection.FindAsync<T>( condition );

                var result =  query.SingleOrDefault();

                return result;
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "GetSingle" );

                throw ex;
            }
        }

        public async Task<T> GetByIdAsync( ObjectId id )
        {
            return await GetSingleAsync( o => o.Id == id );
        }

        public async Task<List<T>> GetListAsync( Expression<Func<T, bool>> condition = null )
        {
            _context.Logger.LogDebug( "REPOSITORY - GetList" );

            try
            {
                IAsyncCursor<T> query;

                if( condition != null )
                {
                    query = await _collection.FindAsync<T>( condition );
                }
                else
                {
                    query = await _collection.FindAsync<T>( _ => true );
                }

                return await query.ToListAsync();
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "GetList" );

                throw ex;
            }
        }

        public async Task<long> CountAsync( Expression<Func<T, bool>> condition = null )
        {
            _context.Logger.LogDebug( "REPOSITORY - Count" );

            try
            {
                if( condition == null ) return _collection.Count( _ => true );

                return await _collection.CountAsync( condition );
            }
            catch( Exception ex )
            {
                _context.Logger.LogError( ex, "Count" );

                throw ex;
            }
        }

        public async Task<bool> ExistsAsync( Expression<Func<T, bool>> condition )
        {
            _context.Logger.LogDebug( "REPOSITORY - Exists" );

            try
            {
                var result = await CountAsync( condition );

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