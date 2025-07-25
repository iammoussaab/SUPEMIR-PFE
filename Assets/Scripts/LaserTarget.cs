using UnityEngine;
using UnityEngine.Events;

namespace MoonBika
{
    public class LaserTarget : MonoBehaviour
    {
        [Header("Target Settings")]
        public bool isActive = true;

        [Header("Target Events")]
        public UnityEvent<Vector3> onLaserHit;
        public UnityEvent onLaserHitBegin;
        public UnityEvent onLaserHitEnd;

        private bool wasHitLastFrame = false;
        private AdvancedLaser currentLaser = null;

        void Update()
        {
            bool isHitThisFrame = (currentLaser != null && currentLaser.IsLaserActive());

            if (isHitThisFrame && !wasHitLastFrame)
            {
                onLaserHitBegin?.Invoke();
            }
            else if (!isHitThisFrame && wasHitLastFrame)
            {
                onLaserHitEnd?.Invoke();
                currentLaser = null;
            }

            wasHitLastFrame = isHitThisFrame;
        }

        public void OnLaserHit(Vector3 hitPosition, Vector3 incomingDirection, AdvancedLaser laser)
        {
            if (!isActive) return;

            currentLaser = laser;
            onLaserHit?.Invoke(hitPosition);
        }

        public bool IsBeingHit()
        {
            return wasHitLastFrame;
        }
    }
}