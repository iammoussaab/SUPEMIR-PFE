using LaserAssetPackage.Scripts.Laser.Drawing.Coloring;
using LaserAssetPackage.Scripts.Laser.Drawing.Controller;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.ResourceLoading;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing
{
    /// <summary>
    /// Handles the drawings of Laser Actors that only use one activation particle system when hit, like <see cref="LaserRepeater"/> and <see cref="NonBlockingLaserReceiver"/>.
    ///
    /// Has an optional locator to set as a location for the particle system. Must be set at editor, or the particles will reset to the
    /// GameObject's location. 
    /// </summary>
    public class SingleTargetLaserDrawer : ForwarderLaserDrawer<IQueryableLaserRelay>
    {
        [Tooltip("Resources path for the particle system that plays when the receiver is hit by a laser.")]
        public string activationParticlesPath;
        
        [Tooltip("Controller from the particle system that plays when the receiver is hit by a laser. Exposed for drag n drop.")]
        public LaserParticleController activationParticlesTemplate;
        
        [Tooltip("Transform to provide an exact place for the activation particle to play at.")]
        public Transform activationParticlesOrigin;

        private readonly ParticleColorBlender _colorBlender = new ParticleColorBlender();
        private Color _particleColor = Color.white;

        [Tooltip("Changes the activation particles intensity")]
        public float activationParticlesIntensityMultiplier = 1f;
        
        protected override void Start()
        {
            base.Start();
            SetUpActivationParticles();
        }

        protected override void OnHitByLaser(IQueryableLaserReceiver sender, LaserHit incomingHit)
        {
            activationParticlesTemplate.Play(_particleColor);
        }

        private void OnAllLaserHitsCeased(IQueryableLaserReceiver sender)
        {
            activationParticlesTemplate.Stop();
        }
        
        private void OnNewEmitterReceived(IQueryableLaserReceiver sender, LaserHit laserHit)
        {
            int id = laserHit.Emitter.GetInstanceID();
            _particleColor = _colorBlender
                .AddEmitterWithColor(id, ColorRegistry.GetParticleColor(id))
                .CalculateColor();
        }

        protected override void OnLaserHitCeased(IQueryableLaserReceiver sender, LaserEmitter emitter)
        {
            base.OnLaserHitCeased(sender, emitter);
            int id = emitter.GetInstanceID();
            _particleColor = _colorBlender
                .RemoveEmitter(id)
                .CalculateColor();
        }

        protected override void SetUpSubscriptions()
        {
            base.SetUpSubscriptions();
            Forwarder.HitByLaser += OnHitByLaser;
            Forwarder.OnEmitterDetached += OnLaserHitCeased;
            Forwarder.OnNewEmitterReceived += OnNewEmitterReceived;
            Forwarder.AllLaserHitsCeased += OnAllLaserHitsCeased;
        }
        
        protected override void TearDownSubscriptions()
        {
            base.TearDownSubscriptions();
            Forwarder.HitByLaser -= OnHitByLaser;
            Forwarder.OnEmitterDetached -= OnLaserHitCeased;
            Forwarder.OnNewEmitterReceived -= OnNewEmitterReceived;
            Forwarder.AllLaserHitsCeased -= OnAllLaserHitsCeased;
        }

        private void SetUpActivationParticles()
        {
            activationParticlesTemplate = new LaserComponentLoader<LaserParticleController>.Builder()
                .WithComponent(activationParticlesTemplate)
                .WithParent(GetActivationParticlesOrigin())
                .WithPrefabPath(activationParticlesPath)
                .Build()
                .GetComponent();
        }

        private Transform GetActivationParticlesOrigin()
        {
            return activationParticlesOrigin != null ? activationParticlesOrigin : transform;
        }
    }
}