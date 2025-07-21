using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Dto
{
    /// <summary>
    /// Contains information for the laser beam to draw.
    /// </summary>
    public struct LaserLineEndpoints
    {
        /// <summary>
        /// The start point of the laser beam.
        /// </summary>
        public Vector3 Origin { get; private set; }
        
        /// <summary>
        /// The end point of the laser beam.
        /// </summary>
        public Vector3 Destination { get; private set; }

        /// <summary>
        /// Creates a new instance from a raycast origin and destination
        /// </summary>
        /// <param name="origin">raycast origin</param>
        /// <param name="destination">raycast destination</param>
        /// <returns>a new instance with the data included</returns>
        public static LaserLineEndpoints Create(Vector3 origin, Vector3 destination)
        {
            return new LaserLineEndpoints()
            {
                Origin = origin,
                Destination = destination
            };
        }
    }
}