using UnityEngine;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest
{
    public abstract class FrameTestRunner : MonoBehaviour, IMonoBehaviourTest
    {
        private int _frameCount = 0;
        public bool IsTestFinished => _frameCount > GetEndFrame();

        protected abstract int GetEndFrame();

        private void Update()
        {
            _frameCount++;
        }
    }
}