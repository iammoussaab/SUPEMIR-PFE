using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Cache
{
    /// <summary>
    /// Provides reuse utility for laser assets so that there is no new object created for every laser interaction.
    /// Laser assets are identified by the <see cref="LaserEmitter"/> they are connected with.
    ///
    /// For example, when an emitter hits an object multiple times, the same beam renderer will be used, as identified by emitter id.
    /// </summary>
    public class LaserDrawerCache<T> where T : Component
    {
        private readonly Dictionary<int, T> _store = new Dictionary<int, T>();
        private readonly T _template;
        private readonly Transform _parent;

        public LaserDrawerCache(T template, Transform parent)
        {
            _template = template;
            _parent = parent;
        }

        /// <summary>
        /// Locates the laser drawer asset that belongs to the given emitter by indexing its store.
        ///
        /// Creates a new asset by using the added template or instantiating a new asset if needed.
        /// </summary>
        /// <param name="emitter">The emitter the drawer asset belongs to</param>
        /// <returns>the found or created drawer asset that belongs to the emitter.</returns>
        public T LocateLaserAsset(LaserEmitter emitter)
        {
            int emitterId = emitter.GetHashCode();
            if (!_store.TryGetValue(emitterId, out var asset))
            {
                asset = CreateAsset();
                _store.Add(emitterId, asset);
            }

            return asset;
        }

        private T CreateAsset()
        {
            return _store.Count == 0 ? _template : Object.Instantiate(_template, _parent);
        }
    }
}