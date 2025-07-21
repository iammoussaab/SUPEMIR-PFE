using LaserAssetPackage.Scripts.Laser.Dto;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Forwards the incoming laser by reflecting it.
    /// </summary>
    public class LaserMirror : LaserRelay
    {
        protected override Vector3 CalculateLaserDirection(LaserHit incomingHit)
        {
            return ReflectedHit(incomingHit);
        }

        protected override Vector3 CalculateLaserOrigin(LaserHit incomingHit)
        {
            return incomingHit.Hit.point + ReflectedHit(incomingHit).normalized * Delta;
        }

        private Vector3 ReflectedHit(LaserHit incomingHit)
        {
            return Vector3.Reflect(incomingHit.Direction, incomingHit.Hit.normal);
        }
    }
}