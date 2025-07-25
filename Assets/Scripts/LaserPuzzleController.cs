using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoonBika
{
    [System.Serializable]
    public class LaserPuzzleController : MonoBehaviour
    {
        [Header("Puzzle Settings")]
        public List<LaserTarget> requiredTargets = new List<LaserTarget>();
        public bool requireAllTargets = true;
        public float activationDelay = 0.5f;
        public List<GameObject> requiredTargetObjects = new List<GameObject>();

        [Header("Line Renderer Settings")]
        public List<LineRenderer> lineRenderersToActivate = new List<LineRenderer>();

        [Header("Mirror Settings")]
        public List<GameObject> mirrorsToControl = new List<GameObject>();

        [Header("Puzzle Events")]
        public UnityEvent onPuzzleSolved;
        public UnityEvent onPuzzleReset;

        public float solveTimer = 0f;
        public bool isPuzzleSolved = false;
        public List<AdvancedLaser> activeLasers = new List<AdvancedLaser>();
        private Coroutine mirrorDeactivationCoroutine = null;

        void Start()
        {
            activeLasers.AddRange(FindObjectsOfType<AdvancedLaser>());

            foreach (LineRenderer lr in lineRenderersToActivate)
            {
                if (lr != null)
                    lr.enabled = false;
            }
        }

        void Update()
        {
            bool allTargetsHit = CheckAllTargetsHit();

            if (allTargetsHit && !isPuzzleSolved)
            {
                solveTimer += Time.deltaTime;

                if (solveTimer >= activationDelay)
                {
                    if (mirrorDeactivationCoroutine == null)
                    {
                        mirrorDeactivationCoroutine = StartCoroutine(SolvePuzzleWithMirrorDeactivation());
                    }
                }
            }
            else if (!allTargetsHit && isPuzzleSolved)
            {
                isPuzzleSolved = false;
                onPuzzleReset?.Invoke();
                solveTimer = 0f;
                if (mirrorDeactivationCoroutine != null)
                {
                    StopCoroutine(mirrorDeactivationCoroutine);
                    mirrorDeactivationCoroutine = null;
                }
                foreach (LineRenderer lr in lineRenderersToActivate)
                {
                    if (lr != null)
                        lr.enabled = false;
                }
                foreach (AdvancedLaser laser in activeLasers)
                {
                    if (laser != null && laser.IsLaserActive())
                    {
                        LineRenderer laserLineRenderer = laser.GetComponent<LineRenderer>();
                        if (laserLineRenderer != null)
                            laserLineRenderer.enabled = true;
                    }
                }
            }
            else if (!allTargetsHit)
            {
                solveTimer = 0f;
            }
        }

        private IEnumerator SolvePuzzleWithMirrorDeactivation()
        {
            foreach (AdvancedLaser laser in activeLasers)
            {
                if (laser != null && laser.IsLaserActive())
                {
                    LineRenderer laserLineRenderer = laser.GetComponent<LineRenderer>();
                    if (laserLineRenderer != null)
                        laserLineRenderer.enabled = false;
                }
            }

            yield return new WaitForSeconds(3f);

            foreach (GameObject mirror in mirrorsToControl)
            {
                if (mirror != null)
                    mirror.SetActive(false);
            }

            foreach (LineRenderer lr in lineRenderersToActivate)
            {
                if (lr != null)
                    lr.enabled = true;
            }

            isPuzzleSolved = true;
            onPuzzleSolved?.Invoke();
            mirrorDeactivationCoroutine = null;
        }

        bool CheckAllTargetsHit()
        {
            if (requireAllTargets)
            {
                foreach (LaserTarget target in requiredTargets)
                {
                    if (target == null || !target.IsBeingHit())
                    {
                        return false;
                    }
                }

                foreach (GameObject targetObj in requiredTargetObjects)
                {
                    bool isHit = false;
                    foreach (AdvancedLaser laser in activeLasers)
                    {
                        if (laser != null && laser.targetObject == targetObj && laser.IsLaserActive())
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
                foreach (LaserTarget target in requiredTargets)
                {
                    if (target != null && target.IsBeingHit())
                    {
                        return true;
                    }
                }

                foreach (GameObject targetObj in requiredTargetObjects)
                {
                    foreach (AdvancedLaser laser in activeLasers)
                    {
                        if (laser != null && laser.targetObject == targetObj && laser.IsLaserActive())
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
            foreach (GameObject mirror in mirrorsToControl)
            {
                if (mirror != null)
                    mirror.SetActive(true);
            }
            foreach (LineRenderer lr in lineRenderersToActivate)
            {
                if (lr != null)
                    lr.enabled = false;
            }
            foreach (AdvancedLaser laser in activeLasers)
            {
                if (laser != null && laser.IsLaserActive())
                {
                    LineRenderer laserLineRenderer = laser.GetComponent<LineRenderer>();
                    if (laserLineRenderer != null)
                        laserLineRenderer.enabled = true;
                }
            }
            if (mirrorDeactivationCoroutine != null)
            {
                StopCoroutine(mirrorDeactivationCoroutine);
                mirrorDeactivationCoroutine = null;
            }
        }
    }
}
