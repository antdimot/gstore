using GStore.Core.Domain;
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
            _mongoDbName = dbname;

            _mongoServerUrl = dburl;

            _client = new MongoClient( _mongoServerUrl );
        }

        public IMongoDatabase Database
        {
            get { return _client.GetDatabase( _mongoDbName ); }
        }

        public void DropDatabase( string dbName )
        {
            _client.DropDatabase( dbName );
        }
    }
}
