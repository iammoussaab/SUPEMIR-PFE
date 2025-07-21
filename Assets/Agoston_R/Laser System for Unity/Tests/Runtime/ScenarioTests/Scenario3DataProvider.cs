using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.Scripting;
using Tests.Player.ScenarioTests.TestData;
using UnityEngine;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests
{
    public class Scenario3DataProvider : LaserActorAware
    {
        private int _frameCounter;

        public LaserEmitter emitter1;
        public LaserEmitter emitter2;
        public LaserMirror mirror1;
        public LaserMirror mirror2;
        public NonBlockingLaserReceiver nbReceiver1;
        public LaserRepeater repeater1;
        public BlockingLaserReceiver receiver1;

        public IDictionary<IQueryableLaserEmitter, EmitterTestData> EmitterTestResults { get; } = new Dictionary<IQueryableLaserEmitter, EmitterTestData>();
        public IDictionary<IQueryableLaserReceiver, RelayTestData> RelayTestData { get; } = new Dictionary<IQueryableLaserReceiver, RelayTestData>();
        public IDictionary<IQueryableLaserReceiver, ReceiverTestData> ReceiverTestData { get; } = new Dictionary<IQueryableLaserReceiver, ReceiverTestData>();

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            foreach (var emitter in new List<LaserEmitter> {emitter1, emitter2})
            {
                var data = new EmitterTestData();
                data.Name = emitter.name;
                EmitterTestResults.Add(emitter, data);
                emitter.EmitterActivated += sender => data.EmitterActivatedMessages++;
                emitter.EmitterDeactivated += sender => data.EmitterDeactivatedMessages++;
                emitter.LaserMiss += (sender, direction) => data.MissedHits++;
                emitter.ChainReturned += (sender, result) => data.HitReceivers.UnionWith(result.AffectedReceivers);
                emitter.LaserHitActor += (sender, hit, receiver) => data.DirectlyHitReceiver = receiver;
                emitter.LaserHitNonActor += (sender, hit) => data.HitNonActors++;
            }

            foreach (var receiver in new List<LaserRelay> {nbReceiver1, mirror1, mirror2, repeater1})
            {
                var data = new RelayTestData();
                data.Name = receiver.name;
                RelayTestData.Add(receiver, data);
                receiver.OnNewEmitterReceived += (sender, hit) => { data.EmittersAttached.Add(hit.Emitter); };
                receiver.LaserHitNonActor += (sender, hit) => data.HitNonActors++;
                receiver.LaserMiss += (sender, hit) => data.MissedHits++;
                receiver.HitByLaser += (sender, hit) => data.HitsByEmitter.Add(hit.Emitter);
                receiver.AllLaserHitsCeased += sender => data.AllHitsCeased++;
                receiver.OnEmitterDetached += (sender, emitter) => data.EmittersDetached.Add(emitter);
            }

            foreach (var receiver in new List<BlockingLaserReceiver> {receiver1})
            {
                var data = new ReceiverTestData();
                data.Name = receiver.name;
                ReceiverTestData.Add(receiver, data);
                receiver.OnNewEmitterReceived += (sender, hit) => { data.EmittersAttached.Add(hit.Emitter); };
                receiver.HitByLaser += (sender, hit) => data.HitsByEmitter.Add(hit.Emitter);
                receiver.AllLaserHitsCeased += sender => data.AllHitsCeased++;
                receiver.OnEmitterDetached += (sender, emitter) => data.EmittersDetached.Add(emitter);
            }
        }

        private void Update()
        {
            _frameCounter++;

            if (_frameCounter == 3)
            {
                foreach (var emitter in new List<LaserEmitter> {emitter1, emitter2})
                {
                    emitter.Deactivate();
                }
            }
        }
    }
}