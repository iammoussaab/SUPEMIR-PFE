using LaserAssetPackage.Scripts.Laser.Dto;

namespace LaserAssetPackage.Scripts.Laser.Logic.Query
{
    /// <summary>
    /// Provides information about a Laser Actor that can shoot a laser (either its own like a <see cref="LaserEmitter"/>
    /// or someone else's like a <see cref="LaserRelay"/>)
    /// </summary>
    public interface IQueryableLaserForwarder : IQueryableLaserActor
    {
        /// <summary>
        /// Invoked when the laser emitted (or forwarded) by this Laser Actor hits another actor.
        ///
        /// Invoked every iteration continuously while the laser makes contact. 
        /// </summary>
        delegate void NotifyLaserHitActor(IQueryableLaserForwarder sender, LaserHit outgoingHit, IQueryableLaserReceiver hitReceiver);

        event NotifyLaserHitActor LaserHitActor;

        /// <summary>
        /// Invoked when the laser emitted (or forwarded) by this Laser Actor hits another collider that is not a Laser Actor.
        ///
        /// Invoked every iteration continuously while the laser makes contact. 
        /// </summary>
        delegate void NotifyLaserHitNonActor(IQueryableLaserForwarder sender, LaserHit outgoingHit);

        event NotifyLaserHitNonActor LaserHitNonActor;

        /// <summary>
        /// Invoked when the laser emitted (or forwarded) by this Laser Actor does not hit any collider.
        ///
        /// Invoked every iteration continuously while the laser makes contact. 
        /// </summary>
        delegate void NotifyLaserMiss(IQueryableLaserForwarder sender, LaserDirection outgoingDirection);

        event NotifyLaserMiss LaserMiss;
    }
}