using UnityEngine;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest
{
    public static class TestUtils
    {
        private const float Eps = 0.01f;
        
        public static bool ApproximatelyEquals(float f1, float f2)
        {
            return Mathf.Abs(f1 - f2) < Eps;
        }

        public static bool ApproximatelyEquals(Vector3 v1, Vector3 v2)
        {
            return Mathf.Abs(v1.x - v2.x) < Eps
                   && Mathf.Abs(v1.y - v2.y) < Eps
                   && Mathf.Abs(v1.z - v2.z) < Eps;
        }
    }
}