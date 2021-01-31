using GStore.Core.Domain;

namespace GStore.API.Models
{
    public class GeoResult
    {
        public string Id { get; set; }

        public double Lon { get; set; }

        public double Lat { get; set; }

        public object Content { get; set; }

        public string Name { get; set; }

        public string[] Tag { get; set; }

        public static GeoResult Create( GeoData data )
        {
            return new GeoResult {
                Id = data.Id.ToString(),
                Lon = data.Location.Coordinates.Longitude,
                Lat = data.Location.Coordinates.Latitude,
                Content = data.Content,
                Name = data.Name,
                //ContentType = data.ContentType,
                Tag = data.Tags
            };
        }
    }
}
