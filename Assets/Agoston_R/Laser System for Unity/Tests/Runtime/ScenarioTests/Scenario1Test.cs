using System.Collections;
using NUnit.Framework;
using Tests.Player.ScenarioTests;
using Tests.Player.ScenarioTests.TestData;
using UnityEngine;
using UnityEngine.TestTools;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests
{
    public class Scenario1Test : RuntimeTest
    {
        protected override string GetSceneName() => "Scenario_1";
        
        [UnityTest]
        public IEnumerator Test_Emitter1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

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
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Emitter2(provider), provider.EmitterTestResults[provider.emitter2]);
        }
        
        [UnityTest]
        public IEnumerator Test_Emitter3()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Emitter3(provider), provider.EmitterTestResults[provider.emitter3]);
        }
        
        [UnityTest]
        public IEnumerator Test_Emitter4()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Emitter4(provider), provider.EmitterTestResults[provider.emitter4]);
        }
        
        [UnityTest]
        public IEnumerator Test_NonblockingReceiver1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_NonblockingReceiver1(provider), provider.RelayTestData[provider.nbReceiver1]);
        }

        [UnityTest]
        public IEnumerator Test_NonblockingReceiver2()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_NonblockingReceiver2(provider), provider.RelayTestData[provider.nbReceiver2]);
        }
        
        [UnityTest]
        public IEnumerator Test_NonblockingReceiver3()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_NonblockingReceiver3(provider), provider.RelayTestData[provider.nbReceiver3]);
        }
        
        [UnityTest]
        public IEnumerator Test_Mirror1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

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
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

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
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Repeater1(provider), provider.RelayTestData[provider.repeater1]);
        }
        
        [UnityTest]
        public IEnumerator Test_Repeater2()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Repeater2(provider), provider.RelayTestData[provider.repeater2]);
        }
        
        [UnityTest]
        public IEnumerator Test_Receiver1()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Receiver1(provider), provider.ReceiverTestData[provider.receiver1]);
        }
        
        [UnityTest]
        public IEnumerator Test_Receiver2()
        {
            // given
            var provider = GameObject.Find("TEST").GetComponent<Scenario1DataProvider>();

            // when
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }

            // then
            Assert.AreEqual(ExpectedData_Receiver2(provider), provider.ReceiverTestData[provider.receiver2]);
        }

        private EmitterTestData ExpectedData_Emitter1(Scenario1DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter1",
                HitReceivers = {provider.mirror1, provider.mirror2, provider.nbReceiver1, provider.repeater1, provider.receiver1},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private EmitterTestData ExpectedData_Emitter2(Scenario1DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter2",
                HitReceivers = {provider.mirror1, provider.repeater1, provider.receiver1},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private EmitterTestData ExpectedData_Emitter3(Scenario1DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter3",
                HitReceivers = {provider.nbReceiver2, provider.repeater2, provider.nbReceiver3},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private EmitterTestData ExpectedData_Emitter4(Scenario1DataProvider provider)
        {
            return new EmitterTestData()
            {
                Name = "emitter4",
                HitReceivers = {provider.nbReceiver3, provider.receiver2},
                MissedHits = 0,
                EmitterActivatedMessages = 0,
                EmitterDeactivatedMessages = 1,
                HitNonActors = 0
            };
        }

        private RelayTestData ExpectedData_Mirror1(Scenario1DataProvider provider)
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

        private RelayTestData ExpectedData_Mirror2(Scenario1DataProvider provider)
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

        private RelayTestData ExpectedData_Repeater1(Scenario1DataProvider provider)
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

        private RelayTestData ExpectedData_Repeater2(Scenario1DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "repeater1",
                EmittersAttached = {provider.emitter3},
                EmittersDetached = {provider.emitter3},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter3}
            };
        }

        private RelayTestData ExpectedData_NonblockingReceiver1(Scenario1DataProvider provider)
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

        private RelayTestData ExpectedData_NonblockingReceiver2(Scenario1DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "nb_receiver2",
                EmittersAttached = {provider.emitter3},
                EmittersDetached = {provider.emitter3},
                MissedHits = 0,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter3}
            };
        }

        private RelayTestData ExpectedData_NonblockingReceiver3(Scenario1DataProvider provider)
        {
            return new RelayTestData()
            {
                Name = "nb_receiver3",
                EmittersAttached = {provider.emitter3, provider.emitter4},
                EmittersDetached = {provider.emitter3, provider.emitter4},
                MissedHits = 1,
                AllHitsCeased = 1,
                HitNonActors = 0,
                HitsByEmitter = {provider.emitter3, provider.emitter4}
            };
        }

        private ReceiverTestData ExpectedData_Receiver1(Scenario1DataProvider provider)
        {
            return new ReceiverTestData()
            {
                Name = "receiver1",
                EmittersAttached = {provider.emitter1, provider.emitter2},
                EmittersDetached = {provider.emitter1, provider.emitter2},
                AllHitsCeased = 1,
                HitsByEmitter = {provider.emitter1, provider.emitter2}
            };
        }

        private ReceiverTestData ExpectedData_Receiver2(Scenario1DataProvider provider)
        {
            return new ReceiverTestData()
            {
                Name = "receiver1",
                EmittersAttached = {provider.emitter4},
                EmittersDetached = {provider.emitter4},
                AllHitsCeased = 1,
                HitsByEmitter = {provider.emitter4}
            };
        }
    }
}