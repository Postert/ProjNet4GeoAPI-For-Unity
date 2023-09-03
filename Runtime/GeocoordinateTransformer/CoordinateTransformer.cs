using ProjNet;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using UnityEngine;

namespace GeocoordinateTransformer
{

    /// <summary>
    /// Contains the coordinates <see cref="UTMCoordinates"/> which correspond to the origin of Unity's coordinate system, the point's WGS84/UTM coordinate reference system <see cref="UTMCrs"/>, and methods for the transformation of coordinates.
    /// </summary>
    public class CoordinateTransformer : MonoBehaviour
    {
        /// <summary>
        /// Contains the WGS84/UTM coordinate reference system of the <see cref="UTMCoordinates"/>.
        /// </summary>
        public UTMCrs UTMCrs = new(utmZone: 32, hemispheres: Hemispheres.Northern);

        /// <summary>
        /// Contains the WGS84/UTM coordinates that correspond to the origin of Unity's coordinate system. 
        /// <remarks>Make sure that the provided point is within a distance smaller than 100 km of the spatial data you want to use in the scene.</remarks>
        /// </summary>
        [Tooltip("WGS84/UTM coordinates, e. g. X: 566600 (E), Y: 5933000 (N), Z: 0 (Altitude)")]
        public UTMCoordinates UTMCoordinates = new(east: 566600, north: 5933000, altitude: 0);


        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType()) { return false; }

            CoordinateTransformer otherSceneOriginUtmCoordinates = other as CoordinateTransformer;

            return (
                this.UTMCrs == otherSceneOriginUtmCoordinates.UTMCrs &&
                this.UTMCoordinates == otherSceneOriginUtmCoordinates.UTMCoordinates
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            Debug.Log("Here");
            return String.Format("SceneOriginUtmCoordinates (with UtmCRS: {0}, UTMCoordinates: {1})", UTMCrs, UTMCoordinates);
        }



        /// <summary>
        /// Use <see cref="CoordinateSystemServices"/> instead.
        /// </summary>
        private CoordinateSystemServices _CoordinateSystemServices;

        /// <summary>
        /// Contains a <see cref="ProjNet.CoordinateSystemServices"/> to create <see cref="ICoordinateTransformation"/>s.
        /// </summary>
        private CoordinateSystemServices CoordinateSystemServices
        {
            get
            {
                if (_CoordinateSystemServices == null)
                {
                    _CoordinateSystemServices = new CoordinateSystemServices();
                }
                return _CoordinateSystemServices;
            }
        }


        /// <summary>
        /// Use <see cref="UTMToGeographicCoordinates"/> instead.
        /// </summary>
        private ICoordinateTransformation _UTMToGeographicCoordinates;

        /// <summary>
        /// Contains a <see cref="ICoordinateTransformation"/> to transform <see cref="GeocoordinateTransformer.UTMCoordinates"/> into <see cref="GeocoordinateTransformer.GeographicCoordinates"/>.
        /// <remarks>Make sure that <see cref="UTMCrs"/>'s <see cref="UTMCrs.UTMZone"/> and <see cref="UTMCrs.Hemisphere"/> and the <see cref="UTMCoordinates"/> are defined before using this transformation.</remarks>
        /// </summary>
        private ICoordinateTransformation UTMToGeographicCoordinates
        {
            get
            {
                if (_UTMToGeographicCoordinates == null)
                {
                    _UTMToGeographicCoordinates = CoordinateSystemServices.CreateTransformation(ProjectedCoordinateSystem.WGS84_UTM(UTMCrs.UTMZone, UTMCrs.IsNorthernHemisphere()), GeographicCoordinateSystem.WGS84);
                }
                return _UTMToGeographicCoordinates;
            }
        }

        /// <summary>
        /// Use <see cref="GeographicToUTMCoordinates"/> instead.
        /// </summary>
        private ICoordinateTransformation _GeographicToUTMCoordinates;

        /// <summary>
        /// Contains a <see cref="ICoordinateTransformation"/> to transform <see cref="GeocoordinateTransformer.GeographicCoordinates"/> into <see cref="GeocoordinateTransformer.UTMCoordinates"/>.
        /// <remarks>Make sure that <see cref="UTMCrs"/>'s <see cref="UTMCrs.UTMZone"/> and <see cref="UTMCrs.Hemisphere"/> and the <see cref="UTMCoordinates"/> are defined before using this transformation.</remarks>
        /// </summary>
        private ICoordinateTransformation GeographicToUTMCoordinates
        {
            get
            {
                if (_GeographicToUTMCoordinates == null)
                {
                    _GeographicToUTMCoordinates = CoordinateSystemServices.CreateTransformation(GeographicCoordinateSystem.WGS84, ProjectedCoordinateSystem.WGS84_UTM(UTMCrs.UTMZone, UTMCrs.IsNorthernHemisphere()));
                }
                return _GeographicToUTMCoordinates;
            }
        }

        /// <summary>
        /// Converts <see cref="GeocoordinateTransformer.UTMCoordinates"/> into <see cref="GeocoordinateTransformer.GeographicCoordinates"/>.
        /// </summary>
        /// <param name="utmCoordinates">Coordinates to be transformed.</param>
        /// <returns><see cref="GeocoordinateTransformer.GeographicCoordinates"/> tranformation result.</returns>
        public GeographicCoordinates GetGeographicCoordinates(UTMCoordinates utmCoordinates)
        {
            double[] coords = UTMToGeographicCoordinates.MathTransform.Transform(new double[] { utmCoordinates.east, utmCoordinates.north });
            return new GeographicCoordinates(latitude: coords[1], longitude: coords[0], altitude: utmCoordinates.altitude);
        }


        /// <summary>
        /// Converts Unity <see cref="Vector3"/> coordinates into <see cref="GeocoordinateTransformer.GeographicCoordinates"/>.
        /// </summary>
        /// <param name="unityCoordinates">Coordinates to be transformed.</param>
        /// <returns><see cref="GeocoordinateTransformer.GeographicCoordinates"/> tranformation result.</returns>
        public GeographicCoordinates GetGeographicCoordinates(Vector3 unityCoordinates)
        {
            UTMCoordinates utmCoordinates = GetUTMCoordinates(unityCoordinates);
            return GetGeographicCoordinates(utmCoordinates);
        }


        /// <summary>
        /// Converts <see cref="GeocoordinateTransformer.GeographicCoordinates"/> into <see cref="GeocoordinateTransformer.UTMCoordinates"/>.
        /// </summary>
        /// <param name="geographicCoordinates">Coordinates to be transformed.</param>
        /// <returns><see cref="GeocoordinateTransformer.UTMCoordinates"/> tranformation result.</returns>
        public UTMCoordinates GetUTMCoordinates(GeographicCoordinates geographicCoordinates)
        {
            double[] coords = GeographicToUTMCoordinates.MathTransform.Transform(new double[] { geographicCoordinates.longitude, geographicCoordinates.latitude });
            return new UTMCoordinates(east: coords[0], north: coords[1], altitude: geographicCoordinates.altitude);
        }


        /// <summary>
        /// Converts <see cref="GeocoordinateTransformer.GeographicCoordinates"/> into Unity <see cref="Vector3"/> coordinates.
        /// </summary>
        /// <param name="geographicCoordinates">Coordinates to be transformed.</param>
        /// <returns>Unity <see cref="Vector3"/> tranformation result.</returns>
        public Vector3 GetUnityCoordinates(GeographicCoordinates geographicCoordinates)
        {
            UTMCoordinates utmCoordinates = GetUTMCoordinates(geographicCoordinates);
            //Debug.LogFormat("utmCoordinates: {0}", utmCoordinates.ToString());
            return GetUnityCoordinates(utmCoordinates);
        }


        /// <summary>
        /// Converts <see cref="GeocoordinateTransformer.UTMCoordinates"/> into Unity <see cref="Vector3"/> coordinates.
        /// </summary>
        /// <param name="utmCoordinates">Coordinates to be transformed.</param>
        /// <returns>Unity <see cref="Vector3"/> tranformation result.</returns>
        /// <exception cref="System.ArgumentException">Thrown when a coordinate component exceeds a distance of 100 km on one of the axes. The latter exceeds the capacity of a float.</exception>
        public Vector3 GetUnityCoordinates(UTMCoordinates utmCoordinates)
        {
            /// Calculate the possition relative to the <seealso cref="SceneOriginUTMCoordinates"/> and check if the calculated coordinate component value can be processed by Unity.

            float eastComponent = (float)(utmCoordinates.east - UTMCoordinates.east);
            float northComponent = (float)(utmCoordinates.north - UTMCoordinates.north);
            float altitudeComponent = (float)(utmCoordinates.altitude - UTMCoordinates.altitude);

            if (
            !(eastComponent < 100 || eastComponent * -1 < 100) ||
            !(northComponent < 100 || northComponent * -1 < 100) ||
            !(altitudeComponent < 100 || altitudeComponent * -1 < 100)
            )
            {
                throw new ArgumentException("Objects with a distance of more than 100 km from the Unity orgigin cannot be processed. Consider to change the SceneOriginUTMCoordinates value.");
            }

            Vector3 positionRelativeToAnchorPoint = new(eastComponent, northComponent, altitudeComponent);

            return SwitchLeftHandedRightHandedCoordinates(positionRelativeToAnchorPoint);
        }




        /// <summary>
        /// Converts Unity <see cref="Vector3"/> coordinates into <see cref="GeocoordinateTransformer.UTMCoordinates"/>.
        /// </summary>
        /// <param name="unityCoordinates">Coordinates to be transformed.</param>
        /// <returns><see cref="GeocoordinateTransformer.UTMCoordinates"/> tranformation result.</returns>
        public UTMCoordinates GetUTMCoordinates(Vector3 unityCoordinates)
        {
            Vector3 pointsSwitchedUnityCoordinate = SwitchLeftHandedRightHandedCoordinates(unityCoordinates);

            double eastComponent = pointsSwitchedUnityCoordinate.x + UTMCoordinates.east;
            double northComponent = pointsSwitchedUnityCoordinate.y + UTMCoordinates.north;
            double altitude = pointsSwitchedUnityCoordinate.z + UTMCoordinates.altitude;

            return new UTMCoordinates(east: eastComponent, north: northComponent, altitude: altitude);
        }


        /// <summary>
        /// Switches coordinates left or right-handed coordinate system values to the other system.
        /// </summary>
        /// <param name="coordinateValues">Coordinates to be transformed.</param>
        /// <returns>Converted Unity <see cref="Vector3"/> coordinates.</returns>
        private Vector3 SwitchLeftHandedRightHandedCoordinates(Vector3 coordinateValues)
        {
            return new Vector3(coordinateValues.x, coordinateValues.z, coordinateValues.y);
        }


        /// <summary>
        /// Unity CoordinateTransformer Singleton instance.
        /// </summary>
        public static CoordinateTransformer instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Debug.LogFormat("New CoordinateTransformer created with {0}", this.ToString());
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}
