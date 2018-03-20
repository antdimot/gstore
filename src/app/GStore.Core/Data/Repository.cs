using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GStore.Core.Data
{
    public class Repository<T> where T : IEntity<ObjectId>
    {
        protected int Limit { get; private set; } = 1000; // max number of items returned from query

        protected DataContext Context { get; private set; }

        protected IMongoCollection<T> Collection { get; private set; }

        protected string CollectionName { get; private set; }

        public Repository( DataContext context )
        {
            Context = context;

            CollectionName = typeof( T ).Name.ToLower();

            Collection = Context.Database.GetCollection<T>( CollectionName );
        }

        #region WRITE METHODS
        public async Task<T> InsertAsync( T instance )
        {
            try
            {
                instance.Id = ObjectId.GenerateNewId();

                await Collection.InsertOneAsync( instance );

                return instance;
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "Insert" );

                throw ex;
            }
        }

        public async void Update( T instance )
        {
            try
            {
                Expression<Func<T, bool>> filter = x => x.Id == instance.Id;

                var update = new ObjectUpdateDefinition<T>( instance );

                await Collection.UpdateOneAsync<T>( filter, update );
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "Update" );

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

                    await Collection.UpdateOneAsync<T>( filter, update );
                }
                else
                {
                    Collection.DeleteOne<T>( filter );
                }
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "Delete" );

                throw ex;
            }
        }

        public void InitColletion()
        {
            Context.Database.DropCollection( CollectionName );
        }
        #endregion

        #region QUERY METHODS
        public async Task<T> GetSingleAsync( Expression<Func<T, bool>> condition )
        {
            Context.Logger.LogDebug( "REPOSITORY - GetSingle" );

            try
            {
                var query = await Collection.FindAsync<T>( condition );

                var result =  query.SingleOrDefault();

                return result;
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "GetSingle" );

                throw ex;
            }
        }

        public async Task<T> GetByIdAsync( ObjectId id )
        {
            return await GetSingleAsync( o => o.Id == id );
        }

        public async Task<IReadOnlyList<T>> GetListAsync( Expression<Func<T, bool>> condition = null )
        {
            Context.Logger.LogDebug( "REPOSITORY - GetList" );

            try
            {
                IAsyncCursor<T> query;

                if( condition == null ) condition = _ => true;

                query = await Collection.Find<T>( condition )
                                         .Limit( Limit )
                                         .ToCursorAsync();

                return await query.ToListAsync();
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "GetList" );

                throw ex;
            }
        }

        public async Task<long> CountAsync( Expression<Func<T, bool>> condition = null )
        {
            Context.Logger.LogDebug( "REPOSITORY - Count" );

            try
            {
                if( condition == null ) condition = _ => true;

                return await Collection.CountAsync( condition );
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "Count" );

                throw ex;
            }
        }

        public async Task<bool> ExistsAsync( Expression<Func<T, bool>> condition )
        {
            Context.Logger.LogDebug( "REPOSITORY - Exists" );

            try
            {
                var result = await CountAsync( condition );

                return result > 0;
            }
            catch( Exception ex )
            {
                Context.Logger.LogError( ex, "Exists" );

                throw ex;
            }
        }   
        #endregion
    }
}