using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class MultiEmitterSingleRelayTest : RuntimeTest
    {
        protected override string GetSceneName() => "MultiEmitterSingleRelayScene";
        
        [UnityTest]
        [Order(1)]
        public IEnumerator TestTwoEmittersWorkWithOneRelay()
        {
            // given
            while (!SceneManager.GetSceneByName("MultiEmitterSingleRelayScene").isLoaded)
            {
            }
            var provider = GameObject.Find("TEST").GetComponent<MultiEmitterSingleRelayTestDataProvider>();
            var receivers = provider.GetLaserActorsByType<IQueryableLaserReceiver>();

            // when
            yield return new MonoBehaviourTest<SingleFrameTestRunner>();

            // then
            Assert.AreEqual(3, provider.GetNumberOfAffectedTargets());
            Assert.True(provider.AreAllTargetsAffectedByAnEmitter());
            Assert.True(provider.AreAllReceiversAffectedByAnEmitter());

            Assert.AreEqual(2, receivers.OfType<NonBlockingLaserReceiver>()
                .First()
                .AttachedEmitterCount);

            Assert.That(receivers.OfType<BlockingLaserReceiver>().All(r => r.AttachedEmitterCount == 1));
            Assert.That(provider.HitsByLaser.Contains(new Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>(
                GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>(),
                GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>())));
            Assert.That(provider.HitsByLaser.Contains(new Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>(
                GameObject.Find("standalone_blocking_receiver").GetComponentInChildren<BlockingLaserReceiver>(),
                GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>())));
            Assert.That(provider.InitialForwardedLasers.Select(t => t.Item2).Contains(GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>()));
            Assert.That(provider.InitialForwardedLasers.Select(t => t.Item2).Contains(GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>()));
            Assert.IsEmpty(provider.ForwardersWithMissedHits);
            Assert.IsEmpty(provider.ForwardersThatHitNonActor);
        }

        [UnityTest]
        [Order(2)]
        public IEnumerator TestOneEmitterRotatedAway()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<MultiEmitterSingleRelayTestDataProvider>();
            var expectedReceiversWithoutEmitters = new HashSet<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>>
            {
                new Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>(GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>(),
                    GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>()),
                new Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>(GameObject.Find("standalone_blocking_receiver").GetComponentInChildren<BlockingLaserReceiver>(),
                    GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>())
            };

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.IsEmpty(expectedReceiversWithoutEmitters.Except(provider.EmittersDetached));
            Assert.AreEqual(2, provider.GetNumberOfAffectedReceivers());
            Assert.AreEqual(0, GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>().AffectedReceivers.Count);
            Assert.AreEqual(2, GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>().AffectedReceivers.Count);
            Assert.AreEqual(GameObject.Find("standalone_blocking_receiver").GetComponentInChildren<BlockingLaserReceiver>(), provider.ReceiversWithHitCeased[0]);
            Assert.False(provider.ForwardedLaserAfterFirstEvent.Select(t => t.Item2).Contains(GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>()));
            Assert.Contains(GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>(), provider.ForwardersWithMissedHits);
            Assert.IsEmpty(provider.ForwardersThatHitNonActor);

            Assert.AreEqual(2, provider.GetNumberOfAffectedTargets());
            Assert.AreEqual(2, provider.GetNumberOfAffectedReceivers());
            Assert.AreEqual(1, provider.GetNumberOfUnaffectedTargets());
            Assert.AreEqual(1, provider.GetNumberOfUnaffectedReceivers());
            Assert.AreEqual(1, provider.GetNumberOfEmittersWithoutTargets());
            Assert.AreEqual(1, provider.GetNumberOfEmittersWithoutReceivers());
        }

        [UnityTest]
        [Order(3)]
        public IEnumerator TestSecondEmitterSwitchedTargets()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<MultiEmitterSingleRelayTestDataProvider>();

            // when
            for (int i = 0; i < 30; i++)
            {
                yield return null;
            }

            // then
            Assert.Contains(GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>(), provider.ForwardersWithMissedHits);

            Assert.That(provider.IsReceiverAffectedByEmitter(
                GameObject.Find("standalone_blocking_receiver").GetComponent<BlockingLaserReceiver>(),
                GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>()
            ));

            Assert.False(provider.IsReceiverAffectedByEmitter(
                GameObject.Find("standalone_blocking_receiver").GetComponent<BlockingLaserReceiver>(),
                GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>()
            ));

            Assert.False(provider.IsReceiverAffectedByEmitter(
                GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>(),
                GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>()
            ));

            Assert.False(provider.IsReceiverAffectedByEmitter(
                GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>(),
                GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>()
            ));

            // 

            Assert.That(provider.DoesEmitterAffectReceiver(
                GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>(),
                GameObject.Find("standalone_blocking_receiver").GetComponent<BlockingLaserReceiver>()
            ));

            Assert.False(provider.DoesEmitterAffectReceiver(
                GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>(),
                GameObject.Find("standalone_blocking_receiver").GetComponent<BlockingLaserReceiver>()
            ));

            Assert.False(provider.DoesEmitterAffectReceiver(
                GameObject.Find("emitter_2").GetComponentInChildren<LaserEmitter>(),
                GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>()
            ));

            Assert.False(provider.DoesEmitterAffectReceiver(
                GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>(),
                GameObject.Find("nonblocking_receiver").GetComponent<NonBlockingLaserReceiver>()
            ));

            Assert.AreEqual(1, provider.GetNumberOfAffectedTargets());
            Assert.AreEqual(1, provider.GetNumberOfAffectedReceivers());
            Assert.AreEqual(2, provider.GetNumberOfUnaffectedTargets());
            Assert.AreEqual(2, provider.GetNumberOfUnaffectedReceivers());
            Assert.AreEqual(1, provider.GetNumberOfEmittersWithoutTargets());
            Assert.AreEqual(1, provider.GetNumberOfEmittersWithoutReceivers());
        }
    }
}