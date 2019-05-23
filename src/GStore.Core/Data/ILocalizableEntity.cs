﻿using MongoDB.Driver.GeoJsonObjectModel;

namespace GStore.Core.Data
{
    public interface ILocalizableEntity<T> : IEntity<T>
    {
        GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        T UserId { get; set; }

        string[] Tags { get; set; }
    }
}
