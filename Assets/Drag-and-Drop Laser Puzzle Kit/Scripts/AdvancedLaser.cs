// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// namespace MoonBika
// {
//     [RequireComponent(typeof(LineRenderer))]
//     public class AdvancedLaser : MonoBehaviour
//     {
//         [Header("Laser Settings")]
//         public LayerMask layerMask;
//         public float defaultLength = 50;
//         public int numOfReflections = 2;
//         public bool enableReflection = true;

//         [Header("Particle Effects")]
//         public Transform laserHitParticlePrefab;
//         public bool showParticlesOnHit = true;

//         [Header("Target System")]
//         public string targetTag = "LaserTarget";
//         public GameObject targetObject; // Added explicit GameObject target

//         [Header("Events")]
//         [SerializeField] public UnityEvent<GameObject> onObjectHit;
//         [SerializeField] public UnityEvent<Vector3> onHitPosition;
//         [SerializeField] public UnityEvent<LaserTarget> onTargetHit;
//         [SerializeField] public UnityEvent onAnyTargetHit;
//         [SerializeField] public UnityEvent onTargetObjectHit; // New event for GameObject target

//         private LineRenderer _lineRenderer;
//         private RaycastHit hit;
//         private Ray ray;
//         private List<Transform> hitParticles = new List<Transform>();
//         private bool isHittingTarget = false;
//         private LaserTarget currentTarget = null;
//         private bool isHittingTargetObject = false; // Track if hitting target GameObject

//         // Start is called before the first frame update
//         void Start()
//         {
//             _lineRenderer = GetComponent<LineRenderer>();

//             // Pre-instantiate particle effects for each potential reflection point
//             if (laserHitParticlePrefab != null && showParticlesOnHit)
//             {
//                 for (int i = 0; i <= numOfReflections; i++)
//                 {
//                     Transform particle = Instantiate(laserHitParticlePrefab, Vector3.zero, Quaternion.identity);
//                     particle.gameObject.SetActive(false);
//                     hitParticles.Add(particle);
//                 }
//             }
//         }

//         // Update is called once per frame
//         void Update()
//         {
//             // Reset target hit status for this frame
//             isHittingTarget = false;
//             isHittingTargetObject = false;
//             currentTarget = null;

//             if (enableReflection)
//             {
//                 ReflectLaser();
//             }
//             else
//             {
//                 NormalLaser();
//             }
//         }

//         void ReflectLaser()
//         {
//             // Deactivate all particles initially
//             foreach (Transform particle in hitParticles)
//             {
//                 if (particle != null)
//                     particle.gameObject.SetActive(false);
//             }

//             ray = new Ray(transform.position, transform.forward);
//             _lineRenderer.positionCount = 1;
//             _lineRenderer.SetPosition(0, transform.position);
//             float remainLength = defaultLength;

//             for (int i = 0; i < numOfReflections; i++)
//             {
//                 // Does the ray intersect any objects excluding the player layer
//                 if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
//                 {
//                     _lineRenderer.positionCount += 1;
//                     _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);

//                     // Show particles at hit point
//                     if (showParticlesOnHit && i < hitParticles.Count && hitParticles[i] != null)
//                     {
//                         hitParticles[i].gameObject.SetActive(true);
//                         hitParticles[i].position = hit.point;
//                         hitParticles[i].rotation = Quaternion.LookRotation(hit.normal);
//                     }

//                     // Handle hit object interactions
//                     ProcessHitObject(hit.collider.gameObject, hit.point, ray.direction);

//                     // Calculate new ray for reflection
//                     remainLength -= Vector3.Distance(ray.origin, hit.point);
//                     ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
//                 }
//                 else
//                 {
//                     _lineRenderer.positionCount += 1;
//                     _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ray.origin + (ray.direction * remainLength));
//                     break;
//                 }
//             }
//         }

//         void NormalLaser()
//         {
//             // Reset particles
//             foreach (Transform particle in hitParticles)
//             {
//                 if (particle != null)
//                     particle.gameObject.SetActive(false);
//             }

//             _lineRenderer.positionCount = 2;
//             _lineRenderer.SetPosition(0, transform.position);

//             // Does the ray intersect any objects excluding the player layer
//             if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
//             {
//                 _lineRenderer.SetPosition(1, hit.point);

//                 // Show particles at hit point
//                 if (showParticlesOnHit && hitParticles.Count > 0 && hitParticles[0] != null)
//                 {
//                     hitParticles[0].gameObject.SetActive(true);
//                     hitParticles[0].position = hit.point;
//                     hitParticles[0].rotation = Quaternion.LookRotation(hit.normal);
//                 }

//                 // Handle hit object interactions
//                 ProcessHitObject(hit.collider.gameObject, hit.point, transform.forward);
//             }
//             else
//             {
//                 _lineRenderer.SetPosition(1, transform.position + transform.forward * defaultLength);
//             }
//         }

//         void ProcessHitObject(GameObject hitObject, Vector3 hitPoint, Vector3 incomingDirection)
//         {
//             if (hitObject == null) return;

//             // Trigger general hit events
//             onObjectHit?.Invoke(hitObject);
//             onHitPosition?.Invoke(hitPoint);

//             // Check for specific target GameObject
//             if (targetObject != null && hitObject == targetObject)
//             {
//                 isHittingTargetObject = true;
//                 onTargetObjectHit?.Invoke();
//             }

//             // Check if this is a laser target by tag
//             LaserTarget target = hitObject.GetComponent<LaserTarget>();
//             if (target != null)
//             {
//                 isHittingTarget = true;
//                 currentTarget = target;
//                 target.OnLaserHit(hitPoint, incomingDirection, this);
//                 onTargetHit?.Invoke(target);
//                 onAnyTargetHit?.Invoke();
//             }

//             // Check if the hit object has a LaserHitObject component
//             LaserHitObject hitComponent = hitObject.GetComponent<LaserHitObject>();
//             if (hitComponent != null)
//             {
//                 hitComponent.OnHit(hitPoint, incomingDirection);
//             }
//         }

//         public bool IsHittingTarget()
//         {
//             return isHittingTarget;
//         }

//         public bool IsHittingTargetObject()
//         {
//             return isHittingTargetObject;
//         }

//         public LaserTarget GetCurrentTarget()
//         {
//             return currentTarget;
//         }
//     }

//     // For objects that can receive laser hits
//     public class LaserHitObject : MonoBehaviour
//     {
//         [Header("Hit Events")]
//         public UnityEvent<Vector3> onLaserHit;
//         public UnityEvent onAnyLaserHit;

//         public void OnHit(Vector3 hitPosition, Vector3 incomingDirection)
//         {
//             // Invoke events
//             onLaserHit?.Invoke(hitPosition);
//             onAnyLaserHit?.Invoke();
//         }
//     }

//     // For special laser target objects (like puzzle elements)
//     public class LaserTarget : MonoBehaviour
//     {
//         [Header("Target Settings")]
//         public bool isActive = true;
//         public string targetType = "default";

//         [Header("Target Events")]
//         public UnityEvent<Vector3> onLaserHit;
//         public UnityEvent onLaserHitBegin;
//         public UnityEvent onLaserHitEnd;

//         private bool wasHitLastFrame = false;
//         private AdvancedLaser currentLaser = null;

//         void Update()
//         {
//             // Check if we were hit this frame
//             bool isHitThisFrame = (currentLaser != null && currentLaser.IsHittingTarget() && currentLaser.GetCurrentTarget() == this);

//             // Fire events for when the laser starts/stops hitting
//             if (isHitThisFrame && !wasHitLastFrame)
//             {
//                 onLaserHitBegin?.Invoke();
//             }
//             else if (!isHitThisFrame && wasHitLastFrame)
//             {
//                 onLaserHitEnd?.Invoke();
//                 currentLaser = null;
//             }

//             wasHitLastFrame = isHitThisFrame;
//         }

//         public void OnLaserHit(Vector3 hitPosition, Vector3 incomingDirection, AdvancedLaser laser)
//         {
//             if (!isActive) return;

//             // Store the laser that hit us
//             currentLaser = laser;

//             // Invoke the hit event
//             onLaserHit?.Invoke(hitPosition);
//         }

//         public bool IsBeingHit()
//         {
//             return wasHitLastFrame;
//         }

//         public string GetTargetType()
//         {
//             return targetType;
//         }
//     }

//     // Example puzzle component that uses laser targets
//     [System.Serializable]
//     public class LaserPuzzleController : MonoBehaviour
//     {
//         [Header("Puzzle Settings")]
//         public List<LaserTarget> requiredTargets = new List<LaserTarget>();
//         public bool requireAllTargets = true;
//         public float activationDelay = 0.5f; // Time all targets must be hit before the puzzle activates
//         public List<GameObject> requiredTargetObjects = new List<GameObject>(); // Added support for GameObject targets

//         [Header("Puzzle Events")]
//         public UnityEvent onPuzzleSolved;
//         public UnityEvent onPuzzleReset;

//         private float solveTimer = 0f;
//         private bool isPuzzleSolved = false;
//         private List<AdvancedLaser> activeLasers = new List<AdvancedLaser>();

//         void Start()
//         {
//             // Find all lasers in the scene
//             AdvancedLaser[] lasers = FindObjectsOfType<AdvancedLaser>();
//             foreach (AdvancedLaser laser in lasers)
//             {
//                 activeLasers.Add(laser);
//             }
//         }

//         void Update()
//         {
//             // Check if all required targets are currently being hit
//             bool allTargetsHit = CheckAllTargetsHit();

//             if (allTargetsHit && !isPuzzleSolved)
//             {
//                 solveTimer += Time.deltaTime;

//                 if (solveTimer >= activationDelay)
//                 {
//                     // Puzzle is solved!
//                     isPuzzleSolved = true;
//                     onPuzzleSolved?.Invoke();
//                 }
//             }
//             else if (!allTargetsHit && isPuzzleSolved)
//             {
//                 // Puzzle was solved but is now reset
//                 isPuzzleSolved = false;
//                 onPuzzleReset?.Invoke();
//                 solveTimer = 0f;
//             }
//             else if (!allTargetsHit)
//             {
//                 // Reset timer if not all targets are hit
//                 solveTimer = 0f;
//             }
//         }

//         bool CheckAllTargetsHit()
//         {
//             // Check component targets
//             if (requireAllTargets)
//             {
//                 // All targets must be hit
//                 foreach (LaserTarget target in requiredTargets)
//                 {
//                     if (target == null || !target.IsBeingHit())
//                     {
//                         return false;
//                     }
//                 }

//                 // Check GameObject targets
//                 foreach (GameObject targetObj in requiredTargetObjects)
//                 {
//                     bool isHit = false;
//                     foreach (AdvancedLaser laser in activeLasers)
//                     {
//                         if (laser.targetObject == targetObj && laser.IsHittingTargetObject())
//                         {
//                             isHit = true;
//                             break;
//                         }
//                     }

//                     if (!isHit)
//                     {
//                         return false;
//                     }
//                 }

//                 return true;
//             }
//             else
//             {
//                 // At least one target must be hit
//                 foreach (LaserTarget target in requiredTargets)
//                 {
//                     if (target != null && target.IsBeingHit())
//                     {
//                         return true;
//                     }
//                 }

//                 // Check GameObject targets
//                 foreach (GameObject targetObj in requiredTargetObjects)
//                 {
//                     foreach (AdvancedLaser laser in activeLasers)
//                     {
//                         if (laser.targetObject == targetObj && laser.IsHittingTargetObject())
//                         {
//                             return true;
//                         }
//                     }
//                 }

//                 return false;
//             }
//         }

//         public void ResetPuzzle()
//         {
//             isPuzzleSolved = false;
//             solveTimer = 0f;
//         }
//     }
// }



using System.Collections;
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
        public bool disableOnTargetHit = false; // New setting to enable/disable this feature

        [Header("Particle Effects")]
        public Transform laserHitParticlePrefab;
        public bool showParticlesOnHit = true;

        [Header("Target System")]
        public string targetTag = "LaserTarget";
        public GameObject targetObject; // Added explicit GameObject target

        [Header("Events")]
        [SerializeField] public UnityEvent<GameObject> onObjectHit;
        [SerializeField] public UnityEvent<Vector3> onHitPosition;
        [SerializeField] public UnityEvent<LaserTarget> onTargetHit;
        [SerializeField] public UnityEvent onAnyTargetHit;
        [SerializeField] public UnityEvent onTargetObjectHit; // New event for GameObject target
        [SerializeField] public UnityEvent onLaserDisabled; // New event for when laser is disabled

        private LineRenderer _lineRenderer;
        private RaycastHit hit;
        private Ray ray;
        private List<Transform> hitParticles = new List<Transform>();
        private bool isHittingTarget = false;
        private LaserTarget currentTarget = null;
        private bool isHittingTargetObject = false; // Track if hitting target GameObject
        private bool isLaserActive = true; // New variable to track laser state

        // Start is called before the first frame update
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            // Pre-instantiate particle effects for each potential reflection point
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

        // Update is called once per frame
        void Update()
        {
            // If laser is disabled, hide the line and particles
            if (!isLaserActive)
            {
                _lineRenderer.positionCount = 0;
                foreach (Transform particle in hitParticles)
                {
                    if (particle != null)
                        particle.gameObject.SetActive(false);
                }
                return;
            }

            // Reset target hit status for this frame
            isHittingTarget = false;
            isHittingTargetObject = false;
            currentTarget = null;

            if (enableReflection)
            {
                ReflectLaser();
            }
            else
            {
                NormalLaser();
            }
        }

        void ReflectLaser()
        {
            // Deactivate all particles initially
            foreach (Transform particle in hitParticles)
            {
                if (particle != null)
                    particle.gameObject.SetActive(false);
            }

            ray = new Ray(transform.position, transform.forward);
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);
            float remainLength = defaultLength;

            for (int i = 0; i < numOfReflections; i++)
            {
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);

                    // Show particles at hit point
                    if (showParticlesOnHit && i < hitParticles.Count && hitParticles[i] != null)
                    {
                        hitParticles[i].gameObject.SetActive(true);
                        hitParticles[i].position = hit.point;
                        hitParticles[i].rotation = Quaternion.LookRotation(hit.normal);
                    }

                    // Handle hit object interactions
                    ProcessHitObject(hit.collider.gameObject, hit.point, ray.direction);

                    // Calculate new ray for reflection
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
            // Reset particles
            foreach (Transform particle in hitParticles)
            {
                if (particle != null)
                    particle.gameObject.SetActive(false);
            }

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
            {
                _lineRenderer.SetPosition(1, hit.point);

                // Show particles at hit point
                if (showParticlesOnHit && hitParticles.Count > 0 && hitParticles[0] != null)
                {
                    hitParticles[0].gameObject.SetActive(true);
                    hitParticles[0].position = hit.point;
                    hitParticles[0].rotation = Quaternion.LookRotation(hit.normal);
                }

                // Handle hit object interactions
                ProcessHitObject(hit.collider.gameObject, hit.point, transform.forward);
            }
            else
            {
                _lineRenderer.SetPosition(1, transform.position + transform.forward * defaultLength);
            }
        }

        void ProcessHitObject(GameObject hitObject, Vector3 hitPoint, Vector3 incomingDirection)
        {
            if (hitObject == null) return;

            // Trigger general hit events
            onObjectHit?.Invoke(hitObject);
            onHitPosition?.Invoke(hitPoint);

            // Check for specific target GameObject
            if (targetObject != null && hitObject == targetObject)
            {
                isHittingTargetObject = true;
                onTargetObjectHit?.Invoke();

                // Disable laser if the setting is enabled
                if (disableOnTargetHit)
                {
                    DisableLaser();
                }
            }

            // Check if this is a laser target by tag
            LaserTarget target = hitObject.GetComponent<LaserTarget>();
            if (target != null)
            {
                isHittingTarget = true;
                currentTarget = target;
                target.OnLaserHit(hitPoint, incomingDirection, this);
                onTargetHit?.Invoke(target);
                onAnyTargetHit?.Invoke();
            }

            // Check if the hit object has a LaserHitObject component
            LaserHitObject hitComponent = hitObject.GetComponent<LaserHitObject>();
            if (hitComponent != null)
            {
                hitComponent.OnHit(hitPoint, incomingDirection);
            }
        }

        // New method to disable the laser
        public void DisableLaser()
        {
            if (isLaserActive)
            {
                isLaserActive = false;
                onLaserDisabled?.Invoke();
            }
        }

        // New method to enable the laser
        public void EnableLaser()
        {
            isLaserActive = true;
        }

        // New method to toggle laser state
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

        // New method to check if laser is active
        public bool IsLaserActive()
        {
            return isLaserActive;
        }

        public bool IsHittingTarget()
        {
            return isHittingTarget;
        }

        public bool IsHittingTargetObject()
        {
            return isHittingTargetObject;
        }

        public LaserTarget GetCurrentTarget()
        {
            return currentTarget;
        }
    }

    // For objects that can receive laser hits
    public class LaserHitObject : MonoBehaviour
    {
        [Header("Hit Events")]
        public UnityEvent<Vector3> onLaserHit;
        public UnityEvent onAnyLaserHit;

        public void OnHit(Vector3 hitPosition, Vector3 incomingDirection)
        {
            // Invoke events
            onLaserHit?.Invoke(hitPosition);
            onAnyLaserHit?.Invoke();
        }
    }

    // For special laser target objects (like puzzle elements)
    public class LaserTarget : MonoBehaviour
    {
        [Header("Target Settings")]
        public bool isActive = true;
        public string targetType = "default";

        [Header("Target Events")]
        public UnityEvent<Vector3> onLaserHit;
        public UnityEvent onLaserHitBegin;
        public UnityEvent onLaserHitEnd;

        private bool wasHitLastFrame = false;
        private AdvancedLaser currentLaser = null;

        void Update()
        {
            // Check if we were hit this frame
            bool isHitThisFrame = (currentLaser != null && currentLaser.IsHittingTarget() && currentLaser.GetCurrentTarget() == this);

            // Fire events for when the laser starts/stops hitting
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

            // Store the laser that hit us
            currentLaser = laser;

            // Invoke the hit event
            onLaserHit?.Invoke(hitPosition);
        }

        public bool IsBeingHit()
        {
            return wasHitLastFrame;
        }

        public string GetTargetType()
        {
            return targetType;
        }
    }

    // Example puzzle component that uses laser targets
    [System.Serializable]
    public class LaserPuzzleController : MonoBehaviour
    {
        [Header("Puzzle Settings")]
        public List<LaserTarget> requiredTargets = new List<LaserTarget>();
        public bool requireAllTargets = true;
        public float activationDelay = 0.5f; // Time all targets must be hit before the puzzle activates
        public List<GameObject> requiredTargetObjects = new List<GameObject>(); // Added support for GameObject targets

        [Header("Puzzle Events")]
        public UnityEvent onPuzzleSolved;
        public UnityEvent onPuzzleReset;

        private float solveTimer = 0f;
        private bool isPuzzleSolved = false;
        private List<AdvancedLaser> activeLasers = new List<AdvancedLaser>();

        void Start()
        {
            // Find all lasers in the scene
            AdvancedLaser[] lasers = FindObjectsOfType<AdvancedLaser>();
            foreach (AdvancedLaser laser in lasers)
            {
                activeLasers.Add(laser);
            }
        }

        void Update()
        {
            // Check if all required targets are currently being hit
            bool allTargetsHit = CheckAllTargetsHit();

            if (allTargetsHit && !isPuzzleSolved)
            {
                solveTimer += Time.deltaTime;

                if (solveTimer >= activationDelay)
                {
                    // Puzzle is solved!
                    isPuzzleSolved = true;
                    onPuzzleSolved?.Invoke();
                }
            }
            else if (!allTargetsHit && isPuzzleSolved)
            {
                // Puzzle was solved but is now reset
                isPuzzleSolved = false;
                onPuzzleReset?.Invoke();
                solveTimer = 0f;
            }
            else if (!allTargetsHit)
            {
                // Reset timer if not all targets are hit
                solveTimer = 0f;
            }
        }

        bool CheckAllTargetsHit()
        {
            // Check component targets
            if (requireAllTargets)
            {
                // All targets must be hit
                foreach (LaserTarget target in requiredTargets)
                {
                    if (target == null || !target.IsBeingHit())
                    {
                        return false;
                    }
                }

                // Check GameObject targets
                foreach (GameObject targetObj in requiredTargetObjects)
                {
                    bool isHit = false;
                    foreach (AdvancedLaser laser in activeLasers)
                    {
                        if (laser.targetObject == targetObj && laser.IsHittingTargetObject())
                        {
                            isHit = true;
                            break;
                        }
                    }

                    if (!isHit)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                // At least one target must be hit
                foreach (LaserTarget target in requiredTargets)
                {
                    if (target != null && target.IsBeingHit())
                    {
                        return true;
                    }
                }

                // Check GameObject targets
                foreach (GameObject targetObj in requiredTargetObjects)
                {
                    foreach (AdvancedLaser laser in activeLasers)
                    {
                        if (laser.targetObject == targetObj && laser.IsHittingTargetObject())
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public void ResetPuzzle()
        {
            isPuzzleSolved = false;
            solveTimer = 0f;
        }
    }
}
