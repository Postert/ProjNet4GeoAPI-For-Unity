using System;
using UnityEngine;

namespace GeocoordinateTransformer
{
    /// <summary>
    /// Contains all variables to specify a WGS84/UTM coordinate refernece system.
    /// </summary>
    [System.Serializable]
    public class UTMCrs
    {
        /// <summary>
        /// Specifies the zone of the WGS84/UTM coordinate system (between 1 and 60).
        /// <remarks>The default WGS84/UTM coordinate system is 32 which is most suitable for Hamburg in Germany.</remarks>
        /// </summary>
        [SerializeField, Range(1, 60)]
        public int UTMZone = 32;

        /// <summary>
        /// Hemisphere of the region. Choose between the northern or southern hemisphere.
        /// <remarks>The default hemisphere setting is Northern as it is suitable for Hamburg in Germany.</remarks>
        /// </summary>
        [SerializeField]
        public Hemispheres Hemisphere = Hemispheres.Northern;

        public UTMCrs(int utmZone, Hemispheres hemispheres)
        {
            if (utmZone < 0 || utmZone > 60) { throw new ArgumentException(String.Format("Invalid UTM zone of {0}", utmZone)); }

            this.UTMZone = utmZone;
            this.Hemisphere = hemispheres;
        }

        /// <summary>
        /// Returns true if the zone is on the northern hemisphere. Otherwise it returns false. 
        /// </summary>
        /// <returns>The default value is true.</returns>
        public bool IsNorthernHemisphere()
        {
            return Hemisphere switch
            {
                Hemispheres.Northern => true,
                Hemispheres.Southern => false,
                _ => true,
            };
        }

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType()) { return false; }

            UTMCrs otherUtmCRS = other as UTMCrs;

            return (
                this.UTMZone == otherUtmCRS.UTMZone &&
                this.Hemisphere == otherUtmCRS.Hemisphere
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("UtmCRS (with UTMZone: {0}, Hemisphere: {1})", UTMZone, Hemisphere);
        }
    }

    public enum Hemispheres
    {
        Northern,
        Southern
    }
}