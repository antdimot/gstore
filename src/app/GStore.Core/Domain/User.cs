using GStore.Core.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GStore.Core.Domain
{
    public class User : IEntity<ObjectId>
    {
        [BsonElement( "_id" )]
        public ObjectId Id { get; set; }

        [BsonElement( "del" )]
        public bool Deleted { get; set; } = false;

        [BsonElement( "enabl" )]
        public bool Enabled { get; set; }

        [BsonElement( "fname" )]
        public string Firstname { get; set; }

        [BsonElement( "lname" )]
        public string Lastname { get; set; }

        [BsonElement( "uname" )]
        public string Username { get; set; }

        [BsonElement( "pword" )]
        public string Password { get; set; }

        [BsonElement( "authz" )]
        public string[] Authorizations { get; set; }
    }
}
