using LaserAssetPackage.Scripts.Laser.Dto;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Target that forwards the incoming laser, creating an illusion that it is pass-through.
    /// </summary>
    public class NonBlockingLaserReceiver : LaserRelay, ILaserTarget
    {
        protected override Vector3 CalculateLaserOrigin(LaserHit incomingHit)
        {
            return incomingHit.Hit.point + incomingHit.Direction.normalized * Delta;
        }
        
        protected override Vector3 CalculateLaserDirection(LaserHit incomingHit)
        {
            return incomingHit.Direction;
        }
    }
}