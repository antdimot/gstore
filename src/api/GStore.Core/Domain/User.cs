using GStore.Core.Data;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Domain
{
    public class User : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }

        public bool Deleted { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
