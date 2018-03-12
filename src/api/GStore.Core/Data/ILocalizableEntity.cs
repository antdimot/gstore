using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GStore.Core.Data
{
    public interface ILocalizableEntity<T> : IEntity<T>
    {
        GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        string[] Tags { get; set; }
    }
}
