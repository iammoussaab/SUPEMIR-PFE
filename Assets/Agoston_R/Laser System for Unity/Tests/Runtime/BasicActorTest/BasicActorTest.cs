using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LaserAssetPackage.Scripts.Laser.Drawing.Controller;
using LaserAssetPackage.Scripts.Laser.Logic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest
{
    /// <summary>
    /// Verifies the simplest behaviour of the actors: the emitters emits and the others respond when hit directly.
    /// </summary>
    public class BasicActorTest : RuntimeTest
    {
        protected override string GetSceneName() => "BasicActorTestScene";

        [UnityTest]
        public IEnumerator LaserMirrorTests()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<RelayTestDataProvider>();
            var testData = provider.TestData[GameObject.Find("mirror").GetComponent<LaserMirror>()];

            // then
            AssertRelayEventsAreCalledInOrder(testData.InvokedEventsInOrder);

            var expectedEmitter = GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>();
            foreach (var emitter in testData.ContactingEmitters)
            {
                Assert.AreEqual(expectedEmitter, emitter);
            }

            Assert.AreEqual(new HashSet<LaserEmitter> {expectedEmitter}, testData.AffectingEmitters);

            Assert.AreEqual(1, testData.AffectingEmitterCount);
            Assert.That(TestUtils.ApproximatelyEquals(expectedEmitter.transform.Find("laser_origin").position, testData.LatestLaserHit.Origin));
            Assert.AreEqual(expectedEmitter, testData.LatestLaserHit.Emitter);
        }

        [UnityTest]
        public IEnumerator NonblockingReceiverTests()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<RelayTestDataProvider>();
            var testData = provider.TestData[GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>()];

            // then
            AssertRelayEventsAreCalledInOrder(testData.InvokedEventsInOrder);

            var expectedEmitter = GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>();
            foreach (var emitter in testData.ContactingEmitters)
            {
                Assert.AreEqual(expectedEmitter, emitter);
            }

            Assert.AreEqual(new HashSet<LaserEmitter> {expectedEmitter}, testData.AffectingEmitters);

            Assert.AreEqual(1, testData.AffectingEmitterCount);
            Assert.That(TestUtils.ApproximatelyEquals(expectedEmitter.transform.Find("laser_origin").position, testData.LatestLaserHit.Origin));
            Assert.AreEqual(expectedEmitter, testData.LatestLaserHit.Emitter);
        }

        [UnityTest]
        public IEnumerator RepeaterCubeTests()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<RelayTestDataProvider>();
            var testData = provider.TestData[GameObject.Find("repeater_cube").GetComponent<LaserRepeater>()];

            // then
            AssertRelayEventsAreCalledInOrder(testData.InvokedEventsInOrder);

            var expectedEmitter = GameObject.Find("emitter_5").GetComponentInChildren<LaserEmitter>();
            foreach (var emitter in testData.ContactingEmitters)
            {
                Assert.AreEqual(expectedEmitter, emitter);
            }

            Assert.AreEqual(new HashSet<LaserEmitter> {expectedEmitter}, testData.AffectingEmitters);

            Assert.AreEqual(1, testData.AffectingEmitterCount);
            Assert.That(TestUtils.ApproximatelyEquals(expectedEmitter.transform.Find("laser_origin").position, testData.LatestLaserHit.Origin));
            Assert.AreEqual(expectedEmitter, testData.LatestLaserHit.Emitter);
        }

        [UnityTest]
        public IEnumerator ActivatedBlockingReceiverTests()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<BlockingReceiverTestDataProvider>();
            var testData = provider.TestData[GameObject.Find("activated_receiver_cube").GetComponentInChildren<BlockingLaserReceiver>()];

            // then
            Assert.AreEqual(new List<string>()
            {
                "OnNewEmitterReceived", "HitByLaser"
            }, testData.InvokedEventsInOrder.Select(m => m.Name).Take(2).ToList());

            var expectedEmitter = GameObject.Find("emitter_3").GetComponentInChildren<LaserEmitter>();
            foreach (var emitter in testData.ContactingEmitters)
            {
                Assert.AreEqual(expectedEmitter, emitter);
            }

            Assert.AreEqual(new HashSet<LaserEmitter> {expectedEmitter}, testData.AffectingEmitters);

            Assert.AreEqual(1, testData.AffectingEmitterCount);
            Assert.That(TestUtils.ApproximatelyEquals(expectedEmitter.transform.Find("laser_origin").position, testData.LatestLaserHit.Origin));
            Assert.AreEqual(expectedEmitter, testData.LatestLaserHit.Emitter);
        }

        [UnityTest]
        public IEnumerator StandaloneBlockingReceiverTests()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<BlockingReceiverTestDataProvider>();
            var testData = provider.TestData[GameObject.Find("standalone_blocking_receiver").GetComponentInChildren<BlockingLaserReceiver>()];

            // then
            Assert.AreEqual(new List<string>()
            {
                "OnNewEmitterReceived", "HitByLaser"
            }, testData.InvokedEventsInOrder.Select(m => m.Name).Take(2).ToList());

            var expectedEmitter = GameObject.Find("standalone_emitter").GetComponentInChildren<LaserEmitter>();
            foreach (var emitter in testData.ContactingEmitters)
            {
                Assert.AreEqual(expectedEmitter, emitter);
            }

            Assert.AreEqual(new HashSet<LaserEmitter> {expectedEmitter}, testData.AffectingEmitters);

            Assert.AreEqual(1, testData.AffectingEmitterCount);
            Assert.That(TestUtils.ApproximatelyEquals(expectedEmitter.transform.Find("laser_origin").position, testData.LatestLaserHit.Origin));
            Assert.AreEqual(expectedEmitter, testData.LatestLaserHit.Emitter);
        }

        [UnityTest]
        public IEnumerator NonActivatedBlockingReceiverTests()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<BlockingReceiverTestDataProvider>();
            var testData = provider.TestData[GameObject.Find("not_activated_receiver_cube").GetComponentInChildren<BlockingLaserReceiver>()];

            // then
            Assert.IsEmpty(testData.InvokedEventsInOrder);
            Assert.IsEmpty(testData.AffectingEmitters);
            Assert.IsEmpty(testData.ContactingEmitters);
            Assert.AreEqual(0, testData.AffectingEmitterCount);
        }

        [UnityTest]
        public IEnumerator NumberOfLaserBeamsTest()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var beams = Object.FindObjectsOfType<LineRenderer>();

            // then
            Assert.AreEqual(9, beams.Length);
        }

        [UnityTest]
        public IEnumerator EmitterActivationParticlesArePlaying()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var emitterActivationParticles = Object.FindObjectsOfType<LaserParticleController>()
                .ToList()
                .FindAll(c => c.name == "activation_particles_3")
                .SelectMany(c => c.GetComponentsInChildren<ParticleSystem>());

            // then
            foreach (var particles in emitterActivationParticles)
            {
                Assert.That(particles.isPlaying);
            }
        }

        [UnityTest]
        public IEnumerator BlockingReceiverParticlesArePlayingCorrectly()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var standaloneReceiver = GameObject.Find("standalone_blocking_receiver").GetComponentInChildren<LaserParticleController>();
            var activeBlockingReceiverParticles = GameObject.Find("activated_receiver_cube").GetComponentInChildren<LaserParticleController>();
            var inactiveReceiver = GameObject.Find("not_activated_receiver_cube").GetComponentInChildren<LaserParticleController>();

            // then
            Assert.That(standaloneReceiver.GetComponentsInChildren<ParticleSystem>().All(s => s.isPlaying));
            Assert.That(activeBlockingReceiverParticles.GetComponentsInChildren<ParticleSystem>().All(s => s.isPlaying));
            Assert.That(inactiveReceiver.GetComponentsInChildren<ParticleSystem>().All(s => !s.isPlaying));
        }

        [UnityTest]
        public IEnumerator EmitterHittingWallPlaysDefaultHitParticles()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var missingEmitterHitParticles = GameObject.Find("emitter_4")
                .GetComponentInChildren<LineRenderer>()
                .GetComponentInChildren<LaserParticleController>()
                .GetComponentsInChildren<ParticleSystem>();

            // then
            Assert.That(missingEmitterHitParticles.All(p => p.isPlaying));
        }

        [UnityTest]
        public IEnumerator EmitterHittingActorsDoesNotPlayDefaultHitParticles()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var missingEmitterHitParticles = GameObject.Find("emitter_1")
                .GetComponentInChildren<LineRenderer>()
                .GetComponentInChildren<LaserParticleController>()
                .GetComponentsInChildren<ParticleSystem>();

            // then
            Assert.That(missingEmitterHitParticles.All(p => !p.isPlaying));
        }

        private void AssertRelayEventsAreCalledInOrder(List<MethodBase> methods)
        {
            Assert.AreEqual(new List<string>
            {
                "OnNewEmitterReceived", "HitByLaser", "LaserMiss"
            }, methods.Select(m => m.Name).Take(3).ToList());
        }
    }
}