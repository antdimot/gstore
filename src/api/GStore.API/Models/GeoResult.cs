using GStore.Core.Domain;
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

        public string[] Tags { get; set; }

        public static GeoResult Create( GeoData data )
        {
            return new GeoResult {
                Id = data.Id.ToString(),
                Longitude = data.Location.Coordinates.Longitude,
                Latitude = data.Location.Coordinates.Latitude,
                Content = data.Content,
                ContentType = data.ContentType,
                Tags = data.Tags
            };
        }
    }
}
