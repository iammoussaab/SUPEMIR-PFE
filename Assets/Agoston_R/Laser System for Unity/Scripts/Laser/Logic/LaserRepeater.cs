using LaserAssetPackage.Scripts.Laser.Dto;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Forwards the incoming laser in the GameObject's forward direction. 
    /// </summary>
    public class LaserRepeater : LaserRelay
    {
        protected override Vector3 CalculateLaserDirection(LaserHit incomingHit)
        {
            return transform.TransformDirection(Vector3.forward);
        }

        protected override Vector3 CalculateLaserOrigin(LaserHit incomingHit)
        {
            return transform.position;
        }
    }
}