using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Dto
{
    /// <summary>
    /// Contains information on a laser hit from the emitter to the target it seeks.
    /// </summary>
    public struct LaserHit
    {
        /// <summary>
        /// The RaycastHit Unity returned after casting the ray. 
        /// </summary>
        public RaycastHit Hit { get; private set; }
        
        /// <summary>
        /// Origin of the raycast.
        /// </summary>
        public Vector3 Origin { get; private set; }
        
        /// <summary>
        /// Direction of the raycast.
        /// </summary>
        public Vector3 Direction { get; private set; }
        
        /// <summary>
        /// The emitter the raycast belongs to.
        /// </summary>
        public LaserEmitter Emitter { get; private set; }
        
        /// <summary>
        /// The laser actors (their InstanceIDs, to be precise) that this emitter has already hit as part of this chain.
        ///
        /// Needed to prevent an infinite loop inside the chain.
        /// </summary>
        public ISet<int> RaycastHistory { get; private set; }
        
        /// <summary>
        /// The remaining distance this raycast has.
        /// </summary>
        public float Distance { get; private set; }
        
        /// <summary>
        /// The layers this raycast takes into account.
        /// </summary>
        public int SupportedLayers { get; private set; }

        /// <summary>
        /// Builder pattern member that replaces the raycast history of this hit.
        /// </summary>
        /// <param name="raycastHistory">the new history</param>
        /// <returns>this hit instance to chain the builder</returns>
        public LaserHit WithHistory(IEnumerable<int> raycastHistory)
        {
            RaycastHistory.Clear();
            RaycastHistory.UnionWith(raycastHistory);
            return this;
        }

        /// <summary>
        /// Builder pattern member that adds the relay to the history of this hit.
        /// </summary>
        /// <param name="objectInstanceId">the InstanceID of the relay we add to the history of this hit</param>
        /// <returns>this hit instance to chain the builder</returns>
        public LaserHit WithHitRelay(int objectInstanceId)
        {
            RaycastHistory.Add(objectInstanceId);
            return this;
        }

        /// <summary>
        /// Used to create a new instance. Initializes the raycast history so it is not null.
        /// </summary>
        /// <param name="hit">the raycast hit</param>
        /// <param name="origin">the origin of the raycast</param>
        /// <param name="direction">direction of the raycast</param>
        /// <param name="emitter">the emitter that sent the ray</param>
        /// <param name="distance">the distance that the ray has left to travel</param>
        /// <returns>the new laser hit instance</returns>
        public static LaserHit Create(RaycastHit hit, Vector3 origin, Vector3 direction, LaserEmitter emitter, float distance, int layers)
        {
            return new LaserHit
            {
                Hit = hit,
                Origin = origin,
                Direction = direction,
                Emitter = emitter,
                RaycastHistory = new HashSet<int>(),
                Distance = distance,
                SupportedLayers = layers
            };
        }
    }
}