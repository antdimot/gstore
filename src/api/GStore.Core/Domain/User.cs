﻿using GStore.Core.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Domain
{
    public class User : IEntity<ObjectId>
    {
        [BsonElement( "_id" )]
        public ObjectId Id { get; set; }

        [BsonElement( "deleted" )]
        public bool Deleted { get; set; }

        [BsonElement( "firstname" )]
        public string Firstname { get; set; }

        [BsonElement( "lastname" )]
        public string Lastname { get; set; }
    }
}
