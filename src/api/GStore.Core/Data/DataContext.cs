using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Data
{
    public class DataContext
    {
        string _mongoServerUrl;
        string _mongoDbName;
        MongoClient _client;

        public DataContext( string dburl, string dbname )
        {
            _mongoServerUrl = dburl;
            _mongoDbName = dbname;
            _client = new MongoClient( _mongoServerUrl );
        }

        public IMongoDatabase GetDatabase() { return _client.GetDatabase( _mongoDbName ); }

        public void DropDatabase( string dbName )
        {
            _client.DropDatabase( dbName );
        }

        public void DropCollection<T>() where T : IEntity<ObjectId>
        {
            var database = GetDatabase();
            var collectionName = typeof( T ).Name.ToLower();

            database.DropCollection( collectionName );
        }
    }
}
