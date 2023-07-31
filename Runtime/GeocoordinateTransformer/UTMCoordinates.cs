using System;

namespace GeocoordinateTransformer
{
    /// <summary>
    /// Contains an easting and northing planar coordinate tuple in a WGS84/UTM coordinate reference system and an altitude value in a user chosen height reference system (e.g. DHHN2016 in Germany).
    /// </summary>
    [System.Serializable]
    public class UTMCoordinates
    {
        /// <summary>
        /// Easting value of the coordinate tuple.
        /// </summary>
        public double east;

        /// <summary>
        /// Northing value of the coordinate tuple.
        /// </summary>
        public double north;

        /// <summary>
        /// Altitude value in a user chosen height reference system (e.g. DHHN2016 in Germany).
        /// <remarks>Note that the altitude value is not associated with the WGS84/UTM coordinate reference system.</remarks>
        /// </summary>
        public double altitude;


        public UTMCoordinates(double east, double north, double altitude)
        {
            this.east = east;
            this.north = north;
            this.altitude = altitude;
        }


        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType()) { return false; }

            UTMCoordinates otherUTMCoordinates = other as UTMCoordinates;

            return (
                this.east == otherUTMCoordinates.east &&
                this.north == otherUTMCoordinates.north &&
                this.altitude == otherUTMCoordinates.altitude
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("UTMCoordinates (with east: {0}, north: {1}, altitude: {2})", east, north, altitude);
        }
    }
}