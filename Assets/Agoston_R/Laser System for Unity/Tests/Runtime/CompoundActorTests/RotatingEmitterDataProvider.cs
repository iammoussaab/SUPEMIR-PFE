using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.Scripting;
using UnityEngine;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.CompoundActorTests
{
    public class RotatingEmitterDataProvider : LaserActorAware
    {
        public int targetExecutionFrameCount = 30;

        private float _degreePerFrame;
        private Rigidbody _emitter;

        public List<IQueryableLaserReceiver> NewEmitterAttachedSenders { get; } = new List<IQueryableLaserReceiver>();
        public List<IQueryableLaserReceiver> EmitterDetachedSenders { get; } = new List<IQueryableLaserReceiver>();

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _emitter = FindLaserActorByRootName<LaserEmitter>("emitter_1").FindLaserRoot().GetComponent<Rigidbody>();
            _degreePerFrame = 60f / targetExecutionFrameCount;

            foreach (var receiver in GetLaserActorsByType<IQueryableLaserReceiver>())
            {
                receiver.OnNewEmitterReceived += (sender, hit) => { NewEmitterAttachedSenders.Add(sender); };
                receiver.OnEmitterDetached += (sender, emitter) => { EmitterDetachedSenders.Add(sender); };
            }
        }

        private void Update()
        {
            if (_emitter.rotation.eulerAngles.y < 300f)
            {
                var increment = Vector3.up * _degreePerFrame;
                _emitter.MoveRotation(Quaternion.Euler(_emitter.rotation.eulerAngles + increment));
            }
        }
    }
}