using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Debug
{
    /// <summary>
    /// Draws debug rays to show where the lasers are cast. 
    /// </summary>
    public class DebugLaserDrawer : MonoBehaviour
    {
        [SerializeField] private float laserRefreshRate = 50f;

        private static readonly Color ForwardTint = Color.blue;
        private static readonly Color HittingRayTint = Color.yellow;
        private static readonly Color MissingRayTint = Color.white;
        
        /// <summary>
        /// Draws a laser that hits a collider. 
        /// </summary>
        /// <param name="origin">the point where the laser starts</param>
        /// <param name="direction">the ray's direction</param>
        /// <param name="distance">the ray's distance</param>
        public void DrawHittingLaser(Vector3 origin, Vector3 direction, float distance)
        {
            UnityEngine.Debug.DrawRay(origin, direction.normalized * distance, HittingRayTint, FpsToDelta(laserRefreshRate));
        }

        /// <summary>
        /// Draws a laser that does not hit a collider.
        ///
        /// The ray is ended based on the given distance, or 1000 units, if no distance is given.
        /// </summary>
        /// <param name="origin">the point where the laser starts</param>
        /// <param name="direction">the ray's direction</param>
        /// <param name="distance">the ray's distance</param>
        public void DrawMissingLaser(Vector3 origin, Vector3 direction, float distance = 1000f)
        {
            UnityEngine.Debug.DrawRay(origin, direction.normalized * distance, MissingRayTint, FpsToDelta(laserRefreshRate));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = ForwardTint;
            Gizmos.DrawRay(transform.position, WorldSpaceForward() * CalculateForwardVectorLength());
        }

        private float CalculateForwardVectorLength()
        {
            float length = 1f;
            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                length = meshFilter.sharedMesh.bounds.extents.z * transform.localScale.z + 0.5f;
            }

            return length;
        }

        private Vector3 WorldSpaceForward()
        {
            return transform.TransformDirection(Vector3.forward);
        }

        private float FpsToDelta(float fps)
        {
            return 1f / fps;
        }
    }
}