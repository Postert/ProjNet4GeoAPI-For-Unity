using System;
using UnityEngine;

namespace GeocoordinateTransformer
{
    /// <summary>
    /// Test class to demonstrate the functionality of CoordinateTransformer. Contains necessary test variables and methods that can be changed and called from Unity's Inspector via the context menu.
    /// </summary>
    public class CoordinateTransformerTest : MonoBehaviour
    {
        /// <summary>
        /// Use <see cref="CoordinateTransformer"/> instead.
        /// </summary>
        [field: SerializeField]
        private CoordinateTransformer _coordinateTransformer;

        /// <summary>
        /// Contains the <see cref="GeocoordinateTransformer.CoordinateTransformer"/> that was created in the Unity Scene.
        /// </summary>
        /// <exception cref="System.ApplicationException">Thrown when there was no <see cref="GeocoordinateTransformer.CoordinateTransformer"/> could be found in the Unity Scene.</exception>
        public CoordinateTransformer CoordinateTransformer
        {
            get
            {
                if (!_coordinateTransformer)
                {
                    _coordinateTransformer = GameObject.FindAnyObjectByType<CoordinateTransformer>();
                    if (!_coordinateTransformer)
                    {
                        throw new ApplicationException("CoordinateTransformer could not be found in scene. Please add one CoordinateTransformer to each szene and provide UTM coordinates for the representing Unity's origin.");
                    }
                }

                return _coordinateTransformer;
            }
            set => _coordinateTransformer = value;
        }

        // Awake is called by Unity
        private void Awake()
        {
            CoordinateTransformer = GameObject.FindAnyObjectByType<CoordinateTransformer>();
            if (!CoordinateTransformer)
            {
                Debug.LogError("CoordinateTransformer could not be found in scene. Please add one CoordinateTransformer to each szene and provide UTM coordinates for the representing Unity's origin.");
            }
        }

        /// <summary>
        /// Predefined and changeable <see cref="GeographicCoordinates"/> for demonstration purpose.
        /// </summary>
        [SerializeField]
        private GeographicCoordinates geographicTestCoordinates = new(latitude: 53.5417104602435, longitude: 10.0051097859429, altitude: 4.25);

        /// <summary>
        /// Predefined and changeable <see cref="UTMCoordinates"/> for demonstration purpose.
        /// </summary>
        [SerializeField]
        private UTMCoordinates utmTestCoordinates = new(east: 566605, north: 5933004, altitude: 3);


        /// <summary>
        /// Places the GameObject, the component is attached to, according to the definition in the <see cref="geographicTestCoordinates"/> field.
        /// </summary>
        [ContextMenu("PlaceAccordingToGeographicCoordinates")]
        private void PlaceAccordingToGeographicCoordinates()
        {
            Debug.LogFormat("Given Geographic coordinates -- latitude: {0}, longitude: {1} and given altitude: {2}", geographicTestCoordinates.latitude, geographicTestCoordinates.longitude, geographicTestCoordinates.altitude);

            Vector3 unityCoordinates = CoordinateTransformer.GetUnityCoordinates(geographicTestCoordinates);
            this.gameObject.transform.position = unityCoordinates;
            Debug.LogFormat("Derived Unity coordinates -- x: {0}, y: {1}, z:{2}\nObject placed accordingly.", unityCoordinates.x, unityCoordinates.y, unityCoordinates.z);
        }


        /// <summary>
        /// Places the GameObject, the component is attached to, according to the definition in the <see cref="utmTestCoordinates"/> field.
        /// </summary>
        [ContextMenu("PlaceAccordingToUTMCoordinates")]
        private void PlaceAccordingToUTMCoordinates()
        {
            Debug.LogFormat("Given UTM coordinates -- E: {0}, N: {1} and given altitude: {2}", utmTestCoordinates.east, utmTestCoordinates.north, utmTestCoordinates.altitude);

            Vector3 unityCoordinates = CoordinateTransformer.GetUnityCoordinates(utmTestCoordinates);
            this.gameObject.transform.position = unityCoordinates;
            Debug.LogFormat("Derived Unity coordinates -- x: {0}, y: {1}, z:{2}\nObject placed accordingly.", unityCoordinates.x, unityCoordinates.y, unityCoordinates.z);
        }


        /// <summary>
        /// Converts the current position in the Unity Scene into <see cref="GeographicCoordinates"/> and prints them in the Unity Console.
        /// </summary>
        [ContextMenu("GetGeographicCoordinatesOfCurrentPlacement")]
        private void GetGeographicCoordinatesOfCurrentPlacement()
        {
            Vector3 unityCoordinates = this.gameObject.transform.position;
            Debug.LogFormat("Given Unity coordinates of placed object -- x: {0}, y: {1}, z:{2}", unityCoordinates.x, unityCoordinates.y, unityCoordinates.z);

            GeographicCoordinates geographicCoordinates = CoordinateTransformer.GetGeographicCoordinates(unityCoordinates);
            Debug.LogFormat("Derived GeographicCoordinates coordinates -- latitude: {0}, longitude: {1} and constant altitude: {2}", geographicCoordinates.latitude, geographicCoordinates.longitude, geographicCoordinates.altitude);
        }


        /// <summary>
        /// Converts the current position in the Unity Scene into <see cref="UTMCoordinates"/> and prints them in the Unity Console.
        /// </summary>
        [ContextMenu("GetUTMCoordinatesOfCurrentPlacement")]
        private void GetUTMCoordinatesOfCurrentPlacement()
        {
            Vector3 unityCoordinates = this.gameObject.transform.position;
            Debug.LogFormat("Given Unity coordinates of placed object -- x: {0}, y: {1}, z:{2}", unityCoordinates.x, unityCoordinates.y, unityCoordinates.z);

            UTMCoordinates utmCoordinates = CoordinateTransformer.GetUTMCoordinates(unityCoordinates);
            Debug.LogFormat("Derived UTM coordinates -- E: {0}, N: {1} and constant altitude: {2}", utmCoordinates.east, utmCoordinates.north, utmCoordinates.altitude);
        }
    }

}