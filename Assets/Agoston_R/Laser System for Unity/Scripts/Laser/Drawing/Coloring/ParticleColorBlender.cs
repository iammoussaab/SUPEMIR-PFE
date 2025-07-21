using System.Collections.Generic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing.Coloring
{
    /// <summary>
    /// Blends between two particle colors and returns the mixed value as a combined particle color.
    ///
    /// Used when multiple laser beams affect a single activation particle system.
    /// </summary>
    public class ParticleColorBlender
    {
        private readonly IDictionary<int, Color> _particleColors = new Dictionary<int, Color>();

        /// <summary>
        /// Adds a new color, identified by its emitter's InstanceID to the blend.
        /// </summary>
        /// <param name="emitterId">the emitter Instance ID whose color is being added</param>
        /// <param name="tint">the color being added to the blender</param>
        /// <returns>the blender instance for the builder pattern</returns>
        public ParticleColorBlender AddEmitterWithColor(int emitterId, Color tint)
        {
            _particleColors.Add(emitterId, tint);
            return this;
        }

        /// <summary>
        /// Remove a new color, identified by its emitter's InstanceID from the blend.
        /// </summary>
        /// <param name="emitterId">the emitter's InstanceID whose color will be removed</param>
        /// <returns>the blender instance for the builder pattern</returns>
        public ParticleColorBlender RemoveEmitter(int emitterId)
        {
            _particleColors.Remove(emitterId);
            return this;
        }

        /// <summary>
        /// The build method of the builder: it calculates the mixed colors
        /// </summary>
        /// <returns>the result of the color blend operation</returns>
        public Color CalculateColor()
        {
            return _particleColors.Count == 0 ? Color.white : LerpStoredColors();
        }

        private Color LerpStoredColors()
        {
            Color output = Color.white;
            float t = 1f;
            foreach (var color in _particleColors.Values)
            {
                output = Color.Lerp(output, color, t);
                t = 0.5f;
            }

            return output;
        }

        /// <summary>
        /// Removes every color from the blend.
        /// </summary>
        public void PurgeRegistry()
        {
            _particleColors.Clear();
        }
    }
}