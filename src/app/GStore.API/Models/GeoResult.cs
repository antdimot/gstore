using GStore.Core.Domain;

namespace GStore.API.Models
{
    public class GeoResult
    {
        public double Lon { get; set; }

        public double Lat { get; set; }

        public object Content { get; set; }

        //public string ContentType { get; set; }

        public string[] Tag { get; set; }

        public static GeoResult Create( GeoData data )
        {
            return new GeoResult {
                Lon = data.Location.Coordinates.Longitude,
                Lat = data.Location.Coordinates.Latitude,
                Content = data.Content,
                //ContentType = data.ContentType,
                Tag = data.Tags
            };
        }
    }
}
