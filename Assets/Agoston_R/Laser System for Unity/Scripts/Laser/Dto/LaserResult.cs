using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;

namespace LaserAssetPackage.Scripts.Laser.Dto
{
    /// <summary>
    /// Contains information that is returned back to the emitter upon contact with a receiver.
    /// </summary>
    public struct LaserResult
    {
        /// <summary>
        /// The list of receivers that is affected by this laser emitter (either directly or indirectly via relays).
        /// </summary>
        public List<ILaserReceiver> AffectedReceivers { get; private set; }
        
        /// <summary>
        /// Used to create a new result instance based on a single hit target.
        /// </summary>
        /// <param name="target">the target that was hit. A new collection is created and the target is added to it.</param>
        /// <returns>a new LaserResult instance</returns>
        public static LaserResult Create(ILaserTarget target)
        {
            LaserResult result = new LaserResult
            {
                AffectedReceivers = new List<ILaserReceiver> { target }
            };
            return result;
        }

        /// <summary>
        /// Used to create an empty instance.
        ///
        /// The targets collection is instantiated for later use.
        /// </summary>
        /// <returns>a new, empty instance</returns>
        public static LaserResult Empty()
        {
            LaserResult result = new LaserResult
            {
                AffectedReceivers = new List<ILaserReceiver>()
            };
            return result;
        }
    }
}