using System;

namespace GeocoordinateTransformer
{
    /// <summary>
    /// Contains a latitude and longitude coordinate tuple and an altitude value in a user chosen height reference system (e.g. DHHN2016 in Germany).
    /// </summary>
    [System.Serializable]
    public class GeographicCoordinates
    {
        /// <summary>
        /// Latitude value of the coordinate tuple.
        /// </summary>
        public double latitude;

        /// <summary>
        /// Longitude value of the coordinate tuple.
        /// </summary>
        public double longitude;

        /// <summary>
        /// Altitude value in a user chosen height reference system (e.g. DHHN2016 in Germany).
        /// <remarks>Note that the altitude value is not associated with the WGS84/UTM coordinate reference system.</remarks>
        /// </summary>
        public double altitude;

        public GeographicCoordinates(double latitude, double longitude, double altitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }


        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType()) { return false; }

            GeographicCoordinates othergeographicCoordinates = other as GeographicCoordinates;

            return (
                this.latitude == othergeographicCoordinates.latitude &&
                this.longitude == othergeographicCoordinates.longitude &&
                this.altitude == othergeographicCoordinates.altitude
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("GeographicCoordinates (with latitude: {0}, longitude: {1}, altitude: {2}", latitude, longitude, altitude);
        }
    }
}
