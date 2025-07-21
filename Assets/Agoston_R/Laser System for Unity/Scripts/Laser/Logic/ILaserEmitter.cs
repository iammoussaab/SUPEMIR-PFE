using LaserAssetPackage.Scripts.Laser.Logic.Query;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// A modifiable contract that expresses a laser emitter. 
    /// </summary>
    public interface ILaserEmitter : IQueryableLaserEmitter
    {
        /// <summary>
        /// Used to activate an inactive <see cref="LaserEmitter"/>. Starts the emission coroutine.
        /// </summary>
        public void Activate();

        /// <summary>
        /// Used to deactivate an active <see cref="LaserEmitter"/>. Stops the emission coroutine.
        /// </summary>
        public void Deactivate();
    }
}