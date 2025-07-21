using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Dto;

namespace LaserAssetPackage.Scripts.Laser.Logic.Query
{
    /// <summary>
    /// Provides information about a Laser Actor that can be affected by the system's lasers.
    /// </summary>
    public interface IQueryableLaserReceiver : IQueryableLaserActor
    {
        /// <summary>
        /// Invoked when this receiver is hit by a new receiver it wasn't getting hit by before.
        ///
        /// Invoked once when the new emitter hits this receiver.
        /// </summary>
        delegate void LaserEmitterAttached(IQueryableLaserReceiver sender, LaserHit laserHit);

        event LaserEmitterAttached OnNewEmitterReceived;

        /// <summary>
        /// Invoked when this Laser Actor used to receive a laser but does not anymore.
        ///
        /// Invoked once when the laser stopped hitting this Laser Actor.
        /// </summary>
        delegate void LaserEmitterDetached(IQueryableLaserReceiver sender, LaserEmitter laserEmitter);

        event LaserEmitterDetached OnEmitterDetached;

        /// <summary>
        /// Invoked when this Laser Actor is hit by a <see cref="LaserEmitter"/> or its forwarded laser.
        ///
        /// Invoked every iteration continuously while the other actor's laser made contact with this one.
        /// </summary>
        delegate void NotifyHitByLaser(IQueryableLaserReceiver sender, LaserHit incomingHit);

        event NotifyHitByLaser HitByLaser;

        /// <summary>
        /// Invoked when this Laser Actor is not hit by any <see cref="LaserEmitter"/> anymore.
        ///
        /// Invoked once, after the hit ceased event if there is no emitter affecting this actor.
        /// </summary>
        delegate void NotifyAllHitsCeased(IQueryableLaserReceiver sender);

        event NotifyAllHitsCeased AllLaserHitsCeased;

        /// <summary>
        /// Returns a set of <see cref="LaserEmitter"/>s that are attached to this target.
        ///
        /// Note that this may not be accurate frame by frame due to emitters relying on Coroutines. 
        /// </summary>
        ISet<LaserEmitter> AttachedEmitters { get; }

        /// <summary>
        /// The number of emitters attached to the target. 
        /// </summary>
        int AttachedEmitterCount { get; }
    }
}