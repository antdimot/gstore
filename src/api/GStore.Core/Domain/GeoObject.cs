using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Domain
{
    public class GeoObject : IEntity<ObjectId>
    {
        [BsonElement( "_id" )]
        public ObjectId Id { get; set; }

        [BsonElement( "deleted" )]
        public bool Deleted { get; set; }

        [BsonElement( "data" )]
        public Object ObjectData { get; set; }

        [BsonElement( "Lat" )]
        public double Latitude { get; set; }

        [BsonElement( "Lon" )]
        public double Longitude { get; set; }

        [BsonElement( "name" )]
        public string Name { get; set; }

        [BsonElement( "description" )]
        public string Description { get; set; }
    }
}
