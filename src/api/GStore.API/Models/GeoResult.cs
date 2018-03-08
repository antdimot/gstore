using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GStore.API.Models
{
    public class GeoResult
    {
        public string Id { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public object Content { get; set; }

        public string ContentType { get; set; }
    }
}
