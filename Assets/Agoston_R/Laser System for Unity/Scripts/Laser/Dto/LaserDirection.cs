using LaserAssetPackage.Scripts.Laser.Logic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Dto
{
    /// <summary>
    /// Contains information on a laser hit from 
    /// </summary>
    public struct LaserDirection
    {
        /// <summary>
        /// Origin of the raycast
        /// </summary>
        public Vector3 Origin { get; private set; }
        
        /// <summary>
        /// Direction of the raycast
        /// </summary>
        public Vector3 Direction { get; private set; }
        
        /// <summary>
        /// The emitter the raycast belongs to
        /// </summary>
        public LaserEmitter Emitter { get; private set; }
        
        /// <summary>
        /// The max distance of the raycast, needed to have a limit and to draw an endpoint to the beam.
        /// </summary>
        public float Distance { get; private set; }

        /// <summary>
        /// Creates a new laser direction instance.
        /// </summary>
        /// <param name="origin">origin</param>
        /// <param name="direction">direction</param>
        /// <param name="emitter">emitter</param>
        /// <param name="distance">distance</param>
        /// <returns></returns>
        public static LaserDirection Create(Vector3 origin, Vector3 direction, LaserEmitter emitter, float distance)
        {
            return new LaserDirection
            {
                Origin = origin,
                Direction = direction,
                Emitter = emitter,
                Distance = distance
            };
        }
    }
}