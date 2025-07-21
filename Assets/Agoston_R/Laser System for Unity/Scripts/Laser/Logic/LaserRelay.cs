using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Cache;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Base class for any laser object that will forward the incoming laser.
    /// </summary>
    public abstract class LaserRelay : QueryableLaserReceiver, ILaserReceiver, IQueryableLaserRelay
    {
        public event IQueryableLaserRelay.NotifyLaserHitActor LaserHitActor;
        public event IQueryableLaserRelay.NotifyLaserHitNonActor LaserHitNonActor;
        public event IQueryableLaserRelay.NotifyLaserMiss LaserMiss;
        public override event IQueryableLaserRelay.NotifyHitByLaser HitByLaser;
        public override event IQueryableLaserRelay.NotifyAllHitsCeased AllLaserHitsCeased;
        public override event ILaserReceiver.LaserEmitterAttached OnNewEmitterReceived;
        public override event ILaserReceiver.LaserEmitterDetached OnEmitterDetached;

        protected const float Delta = 0.05f;

        private readonly LaserReceiverCache _receiverCache = LaserReceiverCache.Instance;
        private readonly ISet<LaserEmitter> _attachedEmitters = new HashSet<LaserEmitter>();

        /// <summary>
        /// Contains the <see cref="LaserEmitter"/>s that are currently affecting this target.
        ///
        /// Note that it may not be accurate frame by frame due to the timing of the <see cref="LaserEmitter"/>. 
        /// </summary>
        public override ISet<LaserEmitter> AttachedEmitters => new HashSet<LaserEmitter>(_attachedEmitters);

        public override int AttachedEmitterCount => _attachedEmitters.Count;

        /// <summary>
        /// The direction to which the laser will be forwarded to. 
        /// </summary>
        /// <param name="incomingHit">the incoming laser hit</param>
        /// <returns>the direction of the forwarded laser</returns>
        protected abstract Vector3 CalculateLaserDirection(LaserHit incomingHit);

        /// <summary>
        /// The origin point of the forwarded laser.
        ///
        /// When implementing this be mindful that the origin point should not be inside the collider of the relay GameObject.
        /// </summary>
        /// <param name="incomingHit">the incoming laser hit</param>
        /// <returns>the origin point of the forwarded laser</returns>
        protected abstract Vector3 CalculateLaserOrigin(LaserHit incomingHit);

        LaserResult ILaserReceiver.Hit(LaserHit laserHit)
        {
            if (_attachedEmitters.Add(laserHit.Emitter))
            {
                OnNewEmitterReceived?.Invoke(this, laserHit);
            }

            if (IsCurrentRelayAlreadyHit(laserHit))
            {
                return LaserResult.Empty();
            }

            HitByLaser?.Invoke(this, laserHit);
            return ShootRayAhead(laserHit);
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

        private LaserResult ShootRayAhead(LaserHit laserHit)
        {
            ILaserReceiver hitReceiver = null;
            Vector3 direction = CalculateLaserDirection(laserHit);
            Vector3 origin = CalculateLaserOrigin(laserHit);
            if (Physics.Raycast(origin, direction, out var raycastHit, laserHit.Distance, laserHit.SupportedLayers))
            {
                hitReceiver = PreventSelfCollision(_receiverCache.FindOrCache(raycastHit.collider));
                OnRaycastHit(laserHit, raycastHit, origin, direction, hitReceiver);
            }
            else
            {
                LaserMiss?.Invoke(this, LaserDirection.Create(origin, direction, laserHit.Emitter, laserHit.Distance));
            }
            
            LaserResult result = hitReceiver?.Hit(CreateForwardedLaserHit(raycastHit, origin, direction, laserHit)) ?? LaserResult.Empty(); 
            result.AffectedReceivers.Add(this);
            return result;
        }

        private void OnRaycastHit(LaserHit laserHit, RaycastHit raycastHit, Vector3 origin, Vector3 direction, ILaserReceiver laserReceiver)
        {
            LaserHit newHit = LaserHit.Create(raycastHit, origin, direction, laserHit.Emitter, laserHit.Distance, laserHit.SupportedLayers);
            if (laserReceiver != null)
            {
                LaserHitActor?.Invoke(this, newHit, laserReceiver);
            }
            else
            {
                LaserHitNonActor?.Invoke(this, newHit);
            }
        }

        private LaserHit CreateForwardedLaserHit(RaycastHit raycastHit, Vector3 origin, Vector3 direction, LaserHit incomingHit)
        {
            float remainingDistance = incomingHit.Distance - raycastHit.distance;
            return LaserHit.Create(raycastHit, origin, direction, incomingHit.Emitter, remainingDistance, incomingHit.SupportedLayers)
                .WithHistory(incomingHit.RaycastHistory)
                .WithHitRelay(GetInstanceID());
        }

        private bool IsCurrentRelayAlreadyHit(LaserHit laserHit)
        {
            return laserHit.RaycastHistory.Contains(GetInstanceID());
        }

        private ILaserReceiver PreventSelfCollision(ILaserReceiver foundReceiver)
        {
            return this.Equals(foundReceiver) ? null : foundReceiver;
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
            return $"{nameof(LaserRelay)} :: {FindLaserRoot().name}/{name}";
        }
    }
}