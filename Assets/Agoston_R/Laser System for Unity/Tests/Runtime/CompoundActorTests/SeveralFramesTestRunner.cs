using LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class SeveralFramesTestRunner : FrameTestRunner
    {
        protected override int GetEndFrame() => 60;
    }
}