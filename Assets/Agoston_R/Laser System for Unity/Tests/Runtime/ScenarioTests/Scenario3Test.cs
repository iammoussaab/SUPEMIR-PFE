using System.Collections;
using NUnit.Framework;
using Tests.Player.ScenarioTests;
using Tests.Player.ScenarioTests.TestData;
using UnityEngine;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests
{
    public class Scenario3Test : RuntimeTest
    {
        protected override string GetSceneName() => "Scenario_3";

        [UnityTest]
        public IEnumerator Test_Emitter1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Emitter1(provider), provider.EmitterTestResults[provider.emitter1]);
        }

        [UnityTest]
        public IEnumerator Test_Emitter2()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Emitter2(provider), provider.EmitterTestResults[provider.emitter2]);
        }

        [UnityTest]
        public IEnumerator Test_NonblockingReceiver1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_NonblockingReceiver1(provider), provider.RelayTestData[provider.nbReceiver1]);
        }

        [UnityTest]
        public IEnumerator Test_Mirror1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Mirror1(provider), provider.RelayTestData[provider.mirror1]);
        }

        [UnityTest]
        public IEnumerator Test_Mirror2()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Mirror2(provider), provider.RelayTestData[provider.mirror2]);
        }

        [UnityTest]
        public IEnumerator Test_Repeater1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Repeater1(provider), provider.RelayTestData[provider.repeater1]);
        }

        [UnityTest]
        public IEnumerator Test_Receiver1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario3DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Receiver1(provider), provider.ReceiverTestData[provider.receiver1]);
        }

        private EmitterTestData ExpectedData_Emitter1(Scenario3DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter1",
                HitReceivers = {provider.mirror1, provider.mirror2, provider.nbReceiver1, provider.repeater1},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private EmitterTestData ExpectedData_Emitter2(Scenario3DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter2",
                HitReceivers = {provider.mirror1, provider.repeater1},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private RelayTestData ExpectedData_Mirror1(Scenario3DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "mirror1",
                EmittersAttached = {provider.emitter1, provider.emitter2},
                EmittersDetached = {provider.emitter1, provider.emitter2},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1, provider.emitter2}
            };
        }

        private RelayTestData ExpectedData_Mirror2(Scenario3DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "mirror2",
                EmittersAttached = {provider.emitter1},
                EmittersDetached = {provider.emitter1},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1}
            };
        }

        private RelayTestData ExpectedData_Repeater1(Scenario3DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "repeater1",
                EmittersAttached = {provider.emitter1, provider.emitter2},
                EmittersDetached = {provider.emitter1, provider.emitter2},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1, provider.emitter2}
            };
        }

        private RelayTestData ExpectedData_NonblockingReceiver1(Scenario3DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "nb_receiver1",
                EmittersAttached = {provider.emitter1},
                EmittersDetached = {provider.emitter1},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1}
            };
        }

        private ReceiverTestData ExpectedData_Receiver1(Scenario3DataProvider provider)
        {
            return new ReceiverTestData()
            {
                Name = "receiver1",
                EmittersAttached = { },
                EmittersDetached = { },
                AllHitsCeased = 0,
                HitsByEmitter = { }
            };
        }
    }
}