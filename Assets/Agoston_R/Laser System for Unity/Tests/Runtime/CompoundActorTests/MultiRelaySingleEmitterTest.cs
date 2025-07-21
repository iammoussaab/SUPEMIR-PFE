using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class MultiRelaySingleEmitterTest : RuntimeTest
    {
        protected override string GetSceneName() => "MultipleRelaysSingleEmitterScene";
        
        [UnityTest]
        public IEnumerator NewEmitterInvokedWhereNeeded()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // then
            Assert.AreEqual(4, provider.ReceiversWithEmitterAttached.Count);
        }

        [UnityTest]
        public IEnumerator TestQueriesAreCorrect()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();
            var emitter = GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>();

            // then
            Assert.AreEqual(1, provider.GetNumberOfUnaffectedReceivers());
            Assert.AreEqual(4, provider.GetNumberOfAffectedReceivers());
            Assert.AreEqual(2, provider.GetNumberOfAffectedTargets());
            Assert.AreEqual(1, provider.GetNumberOfUnaffectedTargets());
            Assert.AreEqual(4, provider.GetAffectedReceivers(emitter).Count);
            Assert.IsEmpty(provider.GetEmittersWithoutReceivers());
            Assert.AreEqual(0, provider.GetNumberOfEmittersWithoutReceivers());
            Assert.IsEmpty(provider.GetEmittersWithoutTargets());
            Assert.AreEqual(0, provider.GetNumberOfEmittersWithoutTargets());

            foreach (var receiver in provider.ReceiversWithEmitterAttached)
            {
                Assert.AreEqual(1, provider.GetNumberOfAffectingEmitters(receiver));
            }
        }

        [UnityTest]
        public IEnumerator TestLaserBeamCount()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();

            // when
            var beamCount = Object.FindObjectsOfType<LineRenderer>().Length;

            // then
            Assert.AreEqual(4, beamCount);
        }

        [UnityTest]
        public IEnumerator HitReceiversHaveEmitterAttached()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // when
            var emitter = GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>();
            var emitters = new HashSet<LaserEmitter> {emitter};
            var unaffectedReceiver = GameObject.Find("standalone_blocking_receiver").GetComponent<BlockingLaserReceiver>();

            // then
            foreach (var receiver in provider.ReceiversWithEmitterAttached)
            {
                Assert.AreEqual(emitters, provider.GetAffectingLaserEmitters(receiver));
                Assert.True(provider.DoesEmitterAffectReceiver(emitter, receiver));
                Assert.True(provider.IsReceiverAffectedByEmitter(receiver, emitter));
            }
            
            Assert.False(provider.IsReceiverAffectedByEmitter(unaffectedReceiver, emitter));
        }

        [UnityTest]
        public IEnumerator LaserActorsAreFoundByRootName()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var expectedEmitter = GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>();
            var expectedMirror = GameObject.Find("mirror").GetComponent<LaserMirror>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // when
            var actualEmitter = provider.FindLaserActorByRootName<LaserEmitter>("emitter_1");
            var actualMirror = provider.FindLaserActorByRootName<LaserMirror>("mirror");

            // then
            Assert.AreEqual(expectedEmitter, actualEmitter);
            Assert.AreEqual(expectedMirror, actualMirror);
        }

        [UnityTest]
        public IEnumerator AffectedReceiversAreCorrect()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var emitter = GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // when
            var actualAffectedReceivers = provider.GetAffectedReceivers(emitter);

            // then
            Assert.IsEmpty(provider.ReceiversWithEmitterAttached.Except(actualAffectedReceivers));
        }

        [UnityTest]
        public IEnumerator ReceiverAndTargetCountOfEmitterIsCorrect()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var emitter = GameObject.Find("emitter_1").GetComponentInChildren<LaserEmitter>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // when
            var actualReceiverCount = provider.GetAffectedReceiverCount(emitter);
            var actualTargetCount = provider.GetAffectedTargetCount(emitter);

            // then
            Assert.AreEqual(4, actualReceiverCount);
            Assert.AreEqual(2, actualTargetCount);
        }

        [UnityTest]
        public IEnumerator TotalLaserActorCountIsCorrect()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // when
            var actualActorCount = provider.GetTotalLaserActorCount();

            // then
            Assert.AreEqual(6, actualActorCount);
        }

        [UnityTest]
        public IEnumerator AreAllTargetsHit()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();

            // when
            var actual = provider.AllTargetsHit;
            
            // then
            Assert.False(actual);
        }

        [UnityTest]
        public IEnumerator NoMissedLasers()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();
            
            // expect
            Assert.AreEqual(0, provider.MissedLasersCount);
        }

        [UnityTest]
        public IEnumerator RaycastHistoryIsCorrect()
        {
            // given
            yield return new MonoBehaviourTest<FewFramesTestRunner>();
            var provider = GameObject.Find("TEST").GetComponent<MultiRelaySingleEmitterDataProvider>();
            
            // when
            var lastHistory = provider.LaserReceiverLaserHit.RaycastHistory;
            var relays = provider.Relays;
            
            // then
            Assert.IsEmpty(new HashSet<int>(relays.Select(r => r.GetInstanceID()).Except(lastHistory)));
        }
    }
}