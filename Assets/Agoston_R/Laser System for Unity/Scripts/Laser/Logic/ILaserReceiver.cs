using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic.Query;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Contains methods for the emitters to call when they hit this receiver. Interface exposed for custom receiver implementations.
    ///
    /// If you only want to query for information about this receiver, use <see cref="IQueryableLaserReceiver"/> instead.
    /// </summary>
    public interface ILaserReceiver : IQueryableLaserReceiver
    {
        /// <summary>
        /// Called by a <see cref="LaserEmitter"/> or <see cref="LaserRelay"/> whenever this instance is hit by their laser.
        ///
        /// Interface exposed to make custom implementations available. This method is to be called by a <see cref="LaserEmitter"/> only.
        /// </summary>
        /// <param name="laserHit">Information on the laser hit.</param>
        /// <returns>The result of the laser hit.</returns>
        public LaserResult Hit(LaserHit laserHit);
        
        /// <summary>
        /// Called by the <see cref="LaserEmitter"/> when it stops affecting the given target.
        ///
        /// Interface exposed to make custom implementations available. This method is to be called by a <see cref="LaserEmitter"/> only.
        /// </summary>
        /// <param name="sender">the laser emitter that no longer hits this target.</param>
        public void CeaseHit(LaserEmitter sender);
    }
}
