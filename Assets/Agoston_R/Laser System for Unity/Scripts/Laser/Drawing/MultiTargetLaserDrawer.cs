using LaserAssetPackage.Scripts.Laser.Cache;
using LaserAssetPackage.Scripts.Laser.Drawing.Controller;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.ResourceLoading;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing
{
    /// <summary>
    /// Handles the drawing of Laser Actors that repeat multiple incoming beams, each with its own particle systems, like <see cref="LaserMirror"/>
    /// </summary>
    public class MultiTargetLaserDrawer : ForwarderLaserDrawer<IQueryableLaserRelay>
    {
        [Tooltip("Resources path to the particles that play when the mirror is being hit by a laser beam.")]
        public string mirrorParticlesPath = "Prefabs/mirror_particles";

        [Tooltip("Controller for the particle system that plays when the mirror is hit by a laser beam. Exposed for drag n drop.")]
        public LaserParticleController mirrorParticlesTemplate;

        private LaserDrawerCache<LaserParticleController> _hitParticlesCache;

        protected override void Start()
        {
            base.Start();
            SetUpMirrorParticles();
            SetUpCache();
        }

        private void SetUpCache()
        {
            _hitParticlesCache = new LaserDrawerCache<LaserParticleController>(mirrorParticlesTemplate, transform);
        }

        protected override void SetUpSubscriptions()
        {
            base.SetUpSubscriptions();
            Forwarder.HitByLaser += OnHitByLaser;
            Forwarder.OnEmitterDetached += OnLaserHitCeased;
        }

        protected override void TearDownSubscriptions()
        {
            base.TearDownSubscriptions();
            Forwarder.HitByLaser -= OnHitByLaser;
            Forwarder.OnEmitterDetached -= OnLaserHitCeased;
        }

        private void SetUpMirrorParticles()
        {
            mirrorParticlesTemplate = new LaserComponentLoader<LaserParticleController>.Builder()
                .WithComponent(mirrorParticlesTemplate)
                .WithParent(transform)
                .WithPrefabPath(mirrorParticlesPath)
                .Build()
                .GetComponent();
        }

        protected override void OnHitByLaser(IQueryableLaserReceiver sender, LaserHit incomingHit)
        {
            _hitParticlesCache.LocateLaserAsset(incomingHit.Emitter).Play(incomingHit, ExtractParticleColor(incomingHit.Emitter));
        }

        protected override void OnLaserHitCeased(IQueryableLaserReceiver sender, LaserEmitter laserEmitter)
        {
            base.OnLaserHitCeased(sender, laserEmitter);
            _hitParticlesCache.LocateLaserAsset(laserEmitter).Stop();
        }

        private Color ExtractParticleColor(LaserEmitter emitter)
        {
            return ColorRegistry.GetParticleColor(emitter.GetInstanceID());
        }
    }
}