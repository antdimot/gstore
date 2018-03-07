using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Domain
{
    public class GeoData : IEntity<ObjectId>
    {
        [BsonElement( "_id" )]
        public ObjectId Id { get; set; }

        [BsonElement( "del" )]
        public bool Deleted { get; set; } = false;

        [BsonElement( "content" )]
        public Object Content { get; set; }

        //[BsonElement( "lat" )]
        //public double Latitude { get; set; }

        //[BsonElement( "lon" )]
        //public double Longitude { get; set; }

        [BsonElement( "location" )]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        [BsonElement( "name" )]
        public string Name { get; set; }

        [BsonElement( "descr" )]
        public string Description { get; set; }

        [BsonElement("ctype")]
        public string ContentType { get; set; }

        [BsonElement("uid")]
        public ObjectId UserId { get; set; }
    }
}
