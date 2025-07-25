using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoonBika
{
    [RequireComponent(typeof(LineRenderer))]
    public class AdvancedLaser : MonoBehaviour
    {
        [Header("Laser Settings")]
        public LayerMask layerMask;
        public float defaultLength = 50;
        public int numOfReflections = 2;
        public bool enableReflection = true;
        public bool disableOnTargetHit = false;

        [Header("Particle Effects")]
        public Transform laserHitParticlePrefab;
        public bool showParticlesOnHit = true;

        [Header("Target System")]
        public string targetTag = "LaserTarget";
        public GameObject targetObject;

        [Header("Events")]
        [SerializeField] public UnityEvent<GameObject> onObjectHit;
        [SerializeField] public UnityEvent<Vector3> onHitPosition;
        [SerializeField] public UnityEvent<LaserTarget> onTargetHit;
        [SerializeField] public UnityEvent onAnyTargetHit;
        [SerializeField] public UnityEvent onTargetObjectHit;
        [SerializeField] public UnityEvent onLaserDisabled;

        private LineRenderer _lineRenderer;
        private RaycastHit hit;
        private Ray ray;
        private List<Transform> hitParticles = new List<Transform>();
        private bool isLaserActive = true;

        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            if (laserHitParticlePrefab != null && showParticlesOnHit)
            {
                for (int i = 0; i <= numOfReflections; i++)
                {
                    Transform particle = Instantiate(laserHitParticlePrefab, Vector3.zero, Quaternion.identity);
                    particle.gameObject.SetActive(false);
                    hitParticles.Add(particle);
                }
            }
        }

        void Update()
        {
            if (!isLaserActive)
            {
                _lineRenderer.positionCount = 0;
                DeactivateAllParticles();
                return;
            }

            if (enableReflection)
            {
                ReflectLaser();
            }
            else
            {
                NormalLaser();
            }
        }

        private void DeactivateAllParticles()
        {
            foreach (Transform particle in hitParticles)
            {
                if (particle != null)
                    particle.gameObject.SetActive(false);
            }
        }

        void ReflectLaser()
        {
            DeactivateAllParticles();

            ray = new Ray(transform.position, transform.forward);
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);
            float remainLength = defaultLength;

            for (int i = 0; i < numOfReflections; i++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);

                    if (showParticlesOnHit && i < hitParticles.Count && hitParticles[i] != null)
                    {
                        hitParticles[i].gameObject.SetActive(true);
                        hitParticles[i].position = hit.point;
                        hitParticles[i].rotation = Quaternion.LookRotation(hit.normal);
                    }

                    ProcessHitObject(hit.collider.gameObject, hit.point, ray.direction);

                    remainLength -= Vector3.Distance(ray.origin, hit.point);
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                }
                else
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ray.origin + (ray.direction * remainLength));
                    break;
                }
            }
        }

        void NormalLaser()
        {
            DeactivateAllParticles();

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);

            if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
            {
                _lineRenderer.SetPosition(1, hit.point);

                if (showParticlesOnHit && hitParticles.Count > 0 && hitParticles[0] != null)
                {
                    hitParticles[0].gameObject.SetActive(true);
                    hitParticles[0].position = hit.point;
                    hitParticles[0].rotation = Quaternion.LookRotation(hit.normal);
                }

                ProcessHitObject(hit.collider.gameObject, hit.point, transform.forward);
            }
            else
            {
                _lineRenderer.SetPosition(1, transform.position + transform.forward * defaultLength);
            }
        }

        void ProcessHitObject(GameObject hitObject, Vector3 hitPoint, Vector3 incomingDirection)
        {
            onObjectHit?.Invoke(hitObject);
            onHitPosition?.Invoke(hitPoint);

            if (targetObject != null && hitObject == targetObject)
            {
                onTargetObjectHit?.Invoke();
                if (disableOnTargetHit)
                {
                    DisableLaser();
                }
            }

            LaserTarget target = hitObject.GetComponent<LaserTarget>();
            if (target != null)
            {
                target.OnLaserHit(hitPoint, incomingDirection, this);
                onTargetHit?.Invoke(target);
                onAnyTargetHit?.Invoke();
            }

            LaserHitObject hitComponent = hitObject.GetComponent<LaserHitObject>();
            if (hitComponent != null)
            {
                hitComponent.OnHit(hitPoint, incomingDirection);
            }
        }

        public void DisableLaser()
        {
            if (isLaserActive)
            {
                isLaserActive = false;
                onLaserDisabled?.Invoke();
            }
        }

        public void EnableLaser()
        {
            isLaserActive = true;
        }

        public void ToggleLaser()
        {
            if (isLaserActive)
            {
                DisableLaser();
            }
            else
            {
                EnableLaser();
            }
        }

        public bool IsLaserActive()
        {
            return isLaserActive;
        }
    }
}