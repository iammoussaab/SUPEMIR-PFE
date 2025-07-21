using System;
using System.Collections.Generic;
using System.Linq;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.Scripting;
using UnityEngine;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class MultiEmitterSingleRelayTestDataProvider : LaserActorAware
    {
        public ISet<IQueryableLaserReceiver> Receivers { get; private set; }

        public List<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>> EmittersReceived { get; } =
            new List<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>>();

        public List<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>> EmittersDetached { get; } =
            new List<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>>();

        public List<IQueryableLaserReceiver> ReceiversWithHitCeased { get; } = new List<IQueryableLaserReceiver>();

        public List<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>> HitsByLaser { get; } = new List<Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>>();

        public List<Tuple<IQueryableLaserForwarder, IQueryableLaserEmitter>> InitialForwardedLasers { get; } = new List<Tuple<IQueryableLaserForwarder, IQueryableLaserEmitter>>();

        public List<Tuple<IQueryableLaserForwarder, IQueryableLaserEmitter>> ForwardedLaserAfterFirstEvent { get; } =
            new List<Tuple<IQueryableLaserForwarder, IQueryableLaserEmitter>>();

        public List<IQueryableLaserForwarder> ForwardersWithMissedHits { get; } = new List<IQueryableLaserForwarder>();
        public List<IQueryableLaserForwarder> ForwardersThatHitNonActor { get; } = new List<IQueryableLaserForwarder>();
        
        public int firstActionFrame = 5;
        public int secondActionFrame = 20;
        private int _frameCounter = 0;

        private LaserEmitter _emitter1;
        private LaserEmitter _emitter2;

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _emitter1 = FindLaserActorByRootName<LaserEmitter>("emitter_1");
            _emitter2 = FindLaserActorByRootName<LaserEmitter>("emitter_2");

            _emitter1.LaserMiss += (sender, direction) => { ForwardersWithMissedHits.Add(sender); };
            _emitter2.LaserMiss += (sender, direction) => { ForwardersWithMissedHits.Add(sender); };

            Receivers = GetLaserActorsByType<IQueryableLaserReceiver>();

            foreach (var forwarder in GetLaserActorsByType<IQueryableLaserForwarder>())
            {
                forwarder.LaserHitNonActor += (sender, hit) => { ForwardersThatHitNonActor.Add(sender); };
            }

            foreach (var receiver in Receivers)
            {
                receiver.OnNewEmitterReceived += (sender, hit) => { EmittersReceived.Add(PairOf(sender, hit.Emitter)); };

                receiver.HitByLaser += (sender, hit) => { HitsByLaser.Add(PairOf(sender, hit.Emitter)); };

                receiver.OnEmitterDetached += (sender, emitter) => { EmittersDetached.Add(PairOf(sender, emitter)); };

                receiver.AllLaserHitsCeased += sender => { ReceiversWithHitCeased.Add(sender); };
            }

            var nonblockingReceiver = GetLaserActorsByType<NonBlockingLaserReceiver>().First();

            nonblockingReceiver.LaserHitActor += (sender, hit, receiver) => { InitialForwardedLasers.Add(PairOf(sender, hit.Emitter)); };
            nonblockingReceiver.LaserMiss += (sender, direction) => { ForwardersWithMissedHits.Add(sender); };
        }

        private Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter> PairOf(IQueryableLaserReceiver receiver, IQueryableLaserEmitter emitter)
        {
            return new Tuple<IQueryableLaserReceiver, IQueryableLaserEmitter>(receiver, emitter);
        }

        private Tuple<IQueryableLaserForwarder, IQueryableLaserEmitter> PairOf(IQueryableLaserForwarder forwarder, IQueryableLaserEmitter emitter)
        {
            return new Tuple<IQueryableLaserForwarder, IQueryableLaserEmitter>(forwarder, emitter);
        }

        private void Update()
        {
            _frameCounter++;
            if (_frameCounter == firstActionFrame)
            {
                TurnEmitterToRemoveHit(_emitter1);
            }

            if (_frameCounter == firstActionFrame + 2)
            {
                GetLaserActorsByType<NonBlockingLaserReceiver>().First().LaserHitActor += (sender, hit, receiver) => { ForwardedLaserAfterFirstEvent.Add(PairOf(sender, hit.Emitter)); };
            }

            if (_frameCounter == secondActionFrame)
            {
                TurnEmitterToTarget(_emitter2);
            }
        }

        private void TurnEmitterToTarget(LaserEmitter emitter)
        {
             emitter.FindLaserRoot().transform.LookAt(GameObject.Find("standalone_blocking_receiver").transform);
        }

        private void TurnEmitterToRemoveHit(LaserEmitter emitter)
        {
            var rb = emitter.GetComponentInParent<Rigidbody>();
            rb.rotation = Quaternion.Euler(new Vector3(0, 20, 0) + rb.transform.eulerAngles);
        }
    }
}