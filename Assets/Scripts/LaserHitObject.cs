using UnityEngine;
using UnityEngine.Events;

namespace MoonBika
{
    public class LaserHitObject : MonoBehaviour
    {
        [Header("Hit Events")]
        public UnityEvent<Vector3> onLaserHit;
        public UnityEvent onAnyLaserHit;

        public void OnHit(Vector3 hitPosition, Vector3 incomingDirection)
        {
            onLaserHit?.Invoke(hitPosition);
            onAnyLaserHit?.Invoke();
        }
    }
}