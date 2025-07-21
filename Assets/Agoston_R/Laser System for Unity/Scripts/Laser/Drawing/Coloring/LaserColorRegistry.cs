using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing.Coloring
{
    /// <summary>
    /// Contains info on the selected color of a given <see cref="EmitterLaserDrawer"/> and its matching <see cref="LaserEmitter"/>.
    /// </summary>
    public class LaserColorRegistry
    {
        private readonly IDictionary<int, Color> _beamRegistry = new Dictionary<int, Color>();
        private readonly IDictionary<int, Color> _particleRegistry = new Dictionary<int, Color>();

        private LaserColorRegistry()
        {
        }

        private static LaserColorRegistry _instance;
        
        /// <summary>
        /// Returns the Singleton instance
        /// </summary>
        public static LaserColorRegistry Instance => _instance ??= new LaserColorRegistry();

        /// <summary>
        /// Register the beam color of the emitter.
        /// </summary>
        /// <param name="emitter">the emitter whose beam color will be registered</param>
        /// <param name="color">the color of the beam</param>
        public void RegisterBeamColor(LaserEmitter emitter, Color color)
        {
            _beamRegistry.Add(emitter.GetInstanceID(), color);
        }

        /// <summary>
        /// Register the particle color of the emitter.
        /// </summary>
        /// <param name="emitter">the emitter whose particle color will be registered.</param>
        /// <param name="color">the color of the particles</param>
        public void RegisterParticleColor(LaserEmitter emitter, Color color)
        {
            _particleRegistry.Add(emitter.GetInstanceID(), color);
        }

        /// <summary>
        /// Returns the beam color of an emitter by its Instance ID.
        /// </summary>
        /// <param name="emitterId">Instance ID of the emitter</param>
        /// <returns>the beam color of the emitter</returns>
        public Color GetBeamColor(int emitterId)
        {
            return _beamRegistry.TryGetValue(emitterId, out var storedColor) ? storedColor : Color.white;
        }

        /// <summary>
        /// Returns the particle system color of the given emitter by its Instance ID.
        /// </summary>
        /// <param name="emittedId">the Instance ID of the emitter</param>
        /// <returns>the particle color of the emitter</returns>
        public Color GetParticleColor(int emittedId)
        {
            return _particleRegistry.TryGetValue(emittedId, out var storedColor) ? storedColor : Color.white;
        }
    }
}