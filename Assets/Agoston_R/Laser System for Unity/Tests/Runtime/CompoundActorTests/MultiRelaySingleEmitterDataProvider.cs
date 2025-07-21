using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.Scripting;
using UnityEngine;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class MultiRelaySingleEmitterDataProvider : LaserActorAware
    {
        public ISet<IQueryableLaserReceiver> Receivers { get; private set; } = new HashSet<IQueryableLaserReceiver>();
        public ISet<IQueryableLaserReceiver> ReceiversWithEmitterAttached { get; private set; } = new HashSet<IQueryableLaserReceiver>();
        public ISet<IQueryableLaserRelay> Relays { get; private set; } = new HashSet<IQueryableLaserRelay>();

        public bool AllTargetsHit { get; private set; }
        public int MissedLasersCount { get; private set; }
        public LaserHit LaserReceiverLaserHit { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            Receivers = GetLaserActorsByType<IQueryableLaserReceiver>();
            Relays = GetLaserActorsByType<IQueryableLaserRelay>();
            SubscribeToNewEmitter();
            SubscribeRelays();
            OnAllTargetsHit += SetAllTargetsHitFlag;
            FindLaserActorByRootName<BlockingLaserReceiver>("activated_receiver_cube").OnNewEmitterReceived += (sender, hit) => LaserReceiverLaserHit = hit;
        }

        private void SubscribeRelays()
        {
            foreach (var relay in Relays)
            {
                relay.LaserMiss += (sender, direction) => MissedLasersCount++;
            }
        }

        private void SetAllTargetsHitFlag()
        {
            AllTargetsHit = true;
        }

        private void SubscribeToNewEmitter()
        {
            foreach (var receiver in Receivers)
            {
                receiver.OnNewEmitterReceived += OnNewEmitterReceived;
            }
        }

        private void OnNewEmitterReceived(IQueryableLaserReceiver sender, LaserHit laserhit)
        {
            ReceiversWithEmitterAttached.Add(sender);
        }
    }
}