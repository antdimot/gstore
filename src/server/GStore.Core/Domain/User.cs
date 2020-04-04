using GStore.Core.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GStore.Core.Domain
{
    public class User : IEntity<ObjectId>
    {
        [BsonElement( "_id" )]
        public ObjectId Id { get; set; }

        [BsonElement( "de" )]
        public bool Deleted { get; set; } = false;

        [BsonElement( "en" )]
        public bool Enabled { get; set; }

        [BsonElement( "fn" )]
        public string Firstname { get; set; }

        [BsonElement( "ln" )]
        public string Lastname { get; set; }

        [BsonElement( "un" )]
        public string Username { get; set; }

        [BsonElement( "pw" )]
        public string Password { get; set; }

        [BsonElement( "au" )]
        public string[] Authorizations { get; set; }
    }
}
