using System.Collections.Generic;
using JetBrains.Annotations;
using LaserAssetPackage.Scripts.Laser.Logic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Cache
{
    /// <summary>
    /// Helper class for the <see cref="LaserEmitter"/> to avoid GetComponent invocations every frame.
    /// </summary>
    public sealed class LaserReceiverCache
    {
        private readonly Dictionary<int, ILaserReceiver> _cachedReceivers = new Dictionary<int, ILaserReceiver>();

        private static LaserReceiverCache _instance;
        
        /// <summary>
        /// Returns the Singleton instance.
        /// </summary>
        public static LaserReceiverCache Instance => _instance ??= new LaserReceiverCache();

        private LaserReceiverCache()
        {
        }
        
        /// <summary>
        /// Finds the <see cref="ILaserReceiver"/> in the cache (based on object instance id) or caches the <see cref="ILaserReceiver"/> instance
        /// if it is not yet stored.
        ///
        /// Returns null if the object does not have a <see cref="ILaserReceiver"/> script attached.
        ///
        /// NOTE: the cache stores the GameObject's component even if it is null. The reason for this is to prevent repeated GetComponent calls.
        /// As a consequence, when an object that is already in the cache receives an <see cref="ILaserReceiver"/> component later on that component won't be picked up by the cache.
        /// When adding an <see cref="ILaserReceiver"/> component throughout the game to an object that didn't have it call the <see cref="Purge"/> method.
        /// </summary>
        /// <param name="gameObject">The component that is scanned for a <see cref="ILaserReceiver"/> script.</param>
        /// <returns>the receiver or null if the object does not have a receiver on it.</returns>
        [CanBeNull]
        internal ILaserReceiver FindOrCache(Component gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            if (_cachedReceivers.TryGetValue(gameObject.GetInstanceID(), out var receiver))
            {
                return receiver;
            }
            
            receiver = gameObject.GetComponent<ILaserReceiver>();
            _cachedReceivers.Add(gameObject.GetInstanceID(), receiver);

            return receiver;
        }

        /// <summary>
        /// Clears the laser receiver cache.
        ///
        /// Should be called every time a new laser component is added to a GameObject that already existed in the scene. See <see cref="FindOrCache"/> for more info. 
        /// </summary>
        public void Purge()
        {
            _cachedReceivers.Clear();
        }
    }
}