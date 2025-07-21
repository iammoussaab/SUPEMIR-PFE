using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// A laser target that does not forward the incoming ray (is not pass through). 
    /// </summary>
    public class BlockingLaserReceiver : QueryableLaserReceiver, ILaserTarget
    {
        public override event ILaserTarget.LaserEmitterAttached OnNewEmitterReceived;
        public override event ILaserTarget.LaserEmitterDetached OnEmitterDetached;
        public override event ILaserReceiver.NotifyHitByLaser HitByLaser;
        public override event ILaserReceiver.NotifyAllHitsCeased AllLaserHitsCeased;

        public override ISet<LaserEmitter> AttachedEmitters => _attachedEmitters;
        public override int AttachedEmitterCount => _attachedEmitters.Count;

        private readonly ISet<LaserEmitter> _attachedEmitters = new HashSet<LaserEmitter>();

        LaserResult ILaserReceiver.Hit(LaserHit laserHit)
        {
            if (_attachedEmitters.Add(laserHit.Emitter))
            {
                OnNewEmitterReceived?.Invoke(this, laserHit);
            }
            
            HitByLaser?.Invoke(this, laserHit);

            return LaserResult.Create(this);
        }

        void ILaserReceiver.CeaseHit(LaserEmitter sender)
        {
            if (_attachedEmitters.Remove(sender))
            {
                OnEmitterDetached?.Invoke(this, sender);
            }

            if (_attachedEmitters.Count == 0)
            {
                AllLaserHitsCeased?.Invoke(this);
            }
        }
        
        public override bool Equals(IQueryableLaserActor other)
        {
            return other != null && this.GetInstanceID() == other.GetInstanceID();
        }

        public override Transform FindLaserRoot()
        {
            return GetComponentInParent<LaserActorRoot>().transform;
        }
        
        public override string ToString()
        {
            return $"{nameof(BlockingLaserReceiver)} :: {FindLaserRoot().name}/{name}";
        }
    }
}
