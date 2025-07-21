using System.Collections;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class RotatingEmitterTest : RuntimeTest
    {
        protected override string GetSceneName() => "RotatingEmitterScene";

        [UnityTest]
        public IEnumerator EmitterAttachedAndDetachedAreCorrect()
        {
            // given
            while (!SceneManager.GetSceneByName("RotatingEmitterScene").isLoaded)
            {
            }
            var provider = GameObject.Find("TEST").GetComponent<RotatingEmitterDataProvider>();

            // when
            yield return new MonoBehaviourTest<SeveralFramesTestRunner>();

            // then
            Assert.AreEqual(2, provider.NewEmitterAttachedSenders.Count);
            Assert.AreEqual(2, provider.EmitterDetachedSenders.Count);

            Assert.Contains(provider.FindLaserActorByRootName<IQueryableLaserReceiver>("repeater_cube"), provider.NewEmitterAttachedSenders);
            Assert.Contains(provider.FindLaserActorByRootName<IQueryableLaserReceiver>("activated_receiver_cube"), provider.NewEmitterAttachedSenders);
            Assert.Contains(provider.FindLaserActorByRootName<IQueryableLaserReceiver>("repeater_cube"), provider.EmitterDetachedSenders);
            Assert.Contains(provider.FindLaserActorByRootName<IQueryableLaserReceiver>("activated_receiver_cube"), provider.EmitterDetachedSenders);
        }
    }
}