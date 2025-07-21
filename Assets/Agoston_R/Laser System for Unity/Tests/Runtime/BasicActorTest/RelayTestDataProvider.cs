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
    public class RelayTestDataProvider : LaserActorAware
    {
        public List<LaserRelay> Relays { get; private set; }
        public Dictionary<LaserRelay, BasicTestData> TestData { get; } = new Dictionary<LaserRelay, BasicTestData>();

        protected override void Awake()
        {
            base.Awake();
            FindActors();
            SubscribeToEvents();
        }

        private void FindActors()
        {
            Relays = GetLaserActorsByType<LaserRelay>().ToList();
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
                relay.LaserHitNonActor += LaserHitNonActor;
                relay.AllLaserHitsCeased += AllLaserHitsCeased;
                relay.OnEmitterDetached += OnEmitterDetached;
                relay.LaserHitActor += LaserHitActor;
                relay.HitByLaser += HitByLaser;
                relay.LaserMiss += LaserMiss;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var relay in Relays)
            {
                relay.OnNewEmitterReceived -= OnNewEmitterReceived;
                relay.LaserHitNonActor -= LaserHitNonActor;
                relay.AllLaserHitsCeased -= AllLaserHitsCeased;
                relay.OnEmitterDetached -= OnEmitterDetached;
                relay.LaserHitActor -= LaserHitActor;
                relay.HitByLaser -= HitByLaser;
                relay.LaserMiss -= LaserMiss;
            }
        }

        private void LaserMiss(IQueryableLaserForwarder sender, LaserDirection outgoingdirection)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as LaserRelay].ContactingEmitters.Add(outgoingdirection.Emitter);
        }

        private void HitByLaser(IQueryableLaserReceiver sender, LaserHit incominghit)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as LaserRelay].ContactingEmitters.Add(incominghit.Emitter);
            TestData[sender as LaserRelay].LatestLaserHit = incominghit;
        }

        private void LaserHitActor(IQueryableLaserForwarder sender, LaserHit outgoinghit, IQueryableLaserReceiver receiver)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as LaserRelay].ContactingEmitters.Add(outgoinghit.Emitter);
        }

        private void OnEmitterDetached(IQueryableLaserReceiver sender, LaserEmitter laseremitter)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as LaserRelay].ContactingEmitters.Add(laseremitter);
        }

        private void AllLaserHitsCeased(IQueryableLaserReceiver sender)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
        }

        private void LaserHitNonActor(IQueryableLaserForwarder sender, LaserHit outgoinghit)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as LaserRelay].ContactingEmitters.Add(outgoinghit.Emitter);
        }

        private void OnNewEmitterReceived(IQueryableLaserReceiver sender, LaserHit laserhit)
        {
            TestData[sender as LaserRelay].InvokedEventsInOrder.Add(new StackTrace().GetFrame(0).GetMethod());
            TestData[sender as LaserRelay].ContactingEmitters.Add(laserhit.Emitter);
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