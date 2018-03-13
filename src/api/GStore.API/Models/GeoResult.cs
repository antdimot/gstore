using GStore.Core.Domain;

namespace GStore.API.Models
{
    public class GeoResult
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public object Content { get; set; }

        public string ContentType { get; set; }

        public string[] Tags { get; set; }

        public static GeoResult Create( GeoData data )
        {
            return new GeoResult {
                Longitude = data.Location.Coordinates.Longitude,
                Latitude = data.Location.Coordinates.Latitude,
                Content = data.Content,
                ContentType = data.ContentType,
                Tags = data.Tags
            };
        }
    }
}
