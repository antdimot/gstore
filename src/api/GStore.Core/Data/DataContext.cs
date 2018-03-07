using GStore.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Data
{
    public class DataContext
    {
        public IConfiguration Configuration { get; private set; }
        public ILogger Logger { get; private set; }

        string _mongoServerUrl;
        string _mongoDbName;
        MongoClient _client;

        public DataContext( IConfiguration config, ILogger<DataContext> logger )
        {
            Configuration = config;
            Logger = logger;

            _mongoServerUrl = Configuration.GetSection( "GStore" ).GetValue<string>( "DBconn" );
            _mongoDbName = Configuration.GetSection( "GStore" ).GetValue<string>( "DBname" );

            _client = new MongoClient( _mongoServerUrl );
        }

        public UnitOfWork UnitOfWork { get; set; }

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
