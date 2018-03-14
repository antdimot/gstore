using GStore.Core.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System;

namespace GStore.Core.Domain
{
    public class GeoData : ILocalizableEntity<ObjectId>
    {
        [BsonElement( "_id" )]
        public ObjectId Id { get; set; }

        [BsonElement( "del" )]
        public bool Deleted { get; set; } = false;

        [BsonElement( "content" )]
        public Object Content { get; set; }

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

        [BsonElement( "tags" )]
        public string[] Tags { get; set; }
    }
}
