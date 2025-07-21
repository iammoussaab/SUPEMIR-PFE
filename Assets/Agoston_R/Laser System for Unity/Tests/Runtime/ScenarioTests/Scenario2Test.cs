using System.Collections;
using NUnit.Framework;
using Tests.Player.ScenarioTests.TestData;
using UnityEngine;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests
{
    public class Scenario2Test : RuntimeTest
    {
        protected override string GetSceneName() => "Scenario_2";
        
        [UnityTest]
        public IEnumerator Test_Emitter1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario2DataProvider>();

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
            var provider = GameObject.Find("TEST").GetComponent<Scenario2DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Emitter2(provider), provider.EmitterTestResults[provider.emitter2]);
        }

        [UnityTest]
        public IEnumerator Test_Repeater1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario2DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            // NOTE both emitters affect the receivers. the non repeating nature is only valid for the same emitter.
            Assert.AreEqual(ExpectedData_Repeater1(provider), provider.RelayTestData[provider.repeater1]);
        }

        [UnityTest]
        public IEnumerator Test_Repeater2()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario2DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Repeater2(provider), provider.RelayTestData[provider.repeater2]);
        }

        [UnityTest]
        public IEnumerator Test_Repeater3()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario2DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Repeater3(provider), provider.RelayTestData[provider.repeater3]);
        }

        [UnityTest]
        public IEnumerator Test_Repeater4()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario2DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Repeater4(provider), provider.RelayTestData[provider.repeater4]);
        }

        private EmitterTestData ExpectedData_Emitter1(Scenario2DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter1",
                HitReceivers = {provider.repeater1, provider.repeater2, provider.repeater3, provider.repeater4},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private EmitterTestData ExpectedData_Emitter2(Scenario2DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter2",
                HitReceivers = {provider.repeater2, provider.repeater3, provider.repeater4},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private RelayTestData ExpectedData_Repeater1(Scenario2DataProvider provider)
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

        private RelayTestData ExpectedData_Repeater2(Scenario2DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "repeater2",
                EmittersAttached = {provider.emitter1, provider.emitter2},
                EmittersDetached = {provider.emitter1, provider.emitter2},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1, provider.emitter2}
            };
        }

        private RelayTestData ExpectedData_Repeater3(Scenario2DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "repeater3",
                EmittersAttached = {provider.emitter1, provider.emitter2},
                EmittersDetached = {provider.emitter1, provider.emitter2},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1, provider.emitter2}
            };
        }

        private RelayTestData ExpectedData_Repeater4(Scenario2DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "repeater4",
                EmittersAttached = {provider.emitter1, provider.emitter2},
                EmittersDetached = {provider.emitter1, provider.emitter2},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter1, provider.emitter2}
            };
        }
    }
}