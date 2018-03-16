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

        [BsonElement( "de" )]
        public bool Deleted { get; set; } = false;

        [BsonElement( "cn" )]
        public Object Content { get; set; }

        [BsonElement( "lo" )]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        [BsonElement( "nm" )]
        public string Name { get; set; }

        [BsonElement( "ds" )]
        public string Description { get; set; }

        [BsonElement("ct")]
        public string ContentType { get; set; }

        [BsonElement("ui")]
        public ObjectId UserId { get; set; }

        [BsonElement( "tg" )]
        public string[] Tags { get; set; } = new string[] { };
    }
}
