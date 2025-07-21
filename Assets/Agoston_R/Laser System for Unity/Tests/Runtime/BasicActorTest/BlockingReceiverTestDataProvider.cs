using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.Scripting;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest
{
    public class BlockingReceiverTestDataProvider : LaserActorAware
    {
        public List<BlockingLaserReceiver> Relays { get; private set; }
        public Dictionary<BlockingLaserReceiver, BasicTestData> TestData { get; } = new Dictionary<BlockingLaserReceiver, BasicTestData>();

        protected override void Awake()
        {
            base.Awake();
            FindActors();
            SubscribeToEvents();
        }

        private void FindActors()
        {
            Relays = GetLaserActorsByType<BlockingLaserReceiver>().ToList();
            foreach (var relay in Relays)
            {
                TestData.Add(relay, new BasicTestData());
            }
        }
        
        private void LateUpdate()
        {
            foreach (var relay in Relays)
            {
                TestData[relay].AffectingEmitters = GetAffectingLaserEmitters(relay);
                TestData[relay].AffectingEmitterCount = GetNumberOfAffectingEmitters(relay);    
            }
        }

        private void SubscribeToEvents()
        {
            foreach (var relay in Relays)
            {
                relay.OnNewEmitterReceived += OnNewEmitterReceived;
                relay.AllLaserHitsCeased += AllLaserHitsCeased;
                relay.OnEmitterDetached += OnEmitterDetached;
                relay.HitByLaser += HitByLaser;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var relay in Relays)
            {
                relay.OnNewEmitterReceived -= OnNewEmitterReceived;
                relay.AllLaserHitsCeased -= AllLaserHitsCeased;
                relay.OnEmitterDetached -= OnEmitterDetached;
                relay.HitByLaser -= HitByLaser;
            }
        }

        private void LaserMiss(IQueryableLaserForwarder sender, LaserDirection outgoingdirection)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as BlockingLaserReceiver].ContactingEmitters.Add(outgoingdirection.Emitter);
        }

        private void HitByLaser(IQueryableLaserReceiver sender, LaserHit incominghit)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as BlockingLaserReceiver].ContactingEmitters.Add(incominghit.Emitter);
            TestData[sender as BlockingLaserReceiver].LatestLaserHit = incominghit;
        }

        private void LaserHitActor(IQueryableLaserForwarder sender, LaserHit outgoinghit)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as BlockingLaserReceiver].ContactingEmitters.Add(outgoinghit.Emitter);
        }

        private void OnEmitterDetached(IQueryableLaserReceiver sender, LaserEmitter laseremitter)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as BlockingLaserReceiver].ContactingEmitters.Add(laseremitter);
        }

        private void AllLaserHitsCeased(IQueryableLaserReceiver sender)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
        }

        private void LaserHitNonActor(IQueryableLaserForwarder sender, LaserHit outgoinghit)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as BlockingLaserReceiver].ContactingEmitters.Add(outgoinghit.Emitter);
        }

        private void OnNewEmitterReceived(IQueryableLaserReceiver sender, LaserHit laserhit)
        {
            TestData[sender as BlockingLaserReceiver].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as BlockingLaserReceiver].ContactingEmitters.Add(laserhit.Emitter);
        }

        public class BasicTestData
        {
            public List<MethodBase> InvokedEventsInOrder { get; } = new List<MethodBase>();
            public List<LaserEmitter> ContactingEmitters { get; } = new List<LaserEmitter>();
            public int AffectingEmitterCount { get; set; }
            public ISet<IQueryableLaserEmitter> AffectingEmitters { get; set; }
            public LaserHit LatestLaserHit { get; set; }
        }
    }
}