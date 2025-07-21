using LaserAssetPackage.Scripts.Laser.Logic.Query;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Denotes that this receiver is an end target for an emitter. As an <see cref="ILaserReceiver"/>, contains
    /// methods to be called by the emitter to modify this target. Exposed for custom target implementations.
    ///
    /// If you only want to query information about a target, use <see cref="IQueryableLaserTarget"/> instead.
    /// </summary>
    public interface ILaserTarget : ILaserReceiver, IQueryableLaserTarget
    {
    }
}