using LaserAssetPackage.Scripts.Laser.Cache;
using LaserAssetPackage.Scripts.Laser.Drawing.Coloring;
using LaserAssetPackage.Scripts.Laser.Drawing.Controller;
using LaserAssetPackage.Scripts.Laser.Drawing.Helper;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Exceptions;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.ResourceLoading;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing
{
    /// <summary>
    /// Base class for all laser forwarders (and Laser Actor that can shoot a laser - either its own like a <see cref="LaserEmitter"/>
    /// or someone else's like a <see cref="LaserRelay"/>).
    /// </summary>
    /// <typeparam name="T">The information required by the laser construct for drawing</typeparam>
    public abstract class ForwarderLaserDrawer<T> : MonoBehaviour where T : IQueryableLaserForwarder
    {
        [Tooltip("Resources path to the laser beam prefab.")] 
        public string laserBeamAssetPath = "Prefabs/laser_beam";

        [Tooltip("Resources path to the particles prefab that are played when the laser hits a wall (not an actor).")]
        public string endParticlesAssetPath = "Prefabs/hit_particles";

        [Tooltip("Controller from the particle system that plays when the laser hits a wall (not an actor). Exposed for drag n drop.")]
        public LaserParticleController endParticlesTemplate;

        [Tooltip("Controller that plays the laser beam to visualize the laser. Exposed for drag n drop.")]
        public LaserBeamController beamTemplate;

        private readonly LaserDtoTransformer _laserDtoTransformer = new LaserDtoTransformer();
        protected LaserDrawerCache<LaserBeamController> BeamCache { get; private set; }
        protected LaserColorRegistry ColorRegistry => LaserColorRegistry.Instance;

        private LaserDrawerCache<LaserParticleController> _endParticlesCache;

        protected T Forwarder { get; private set; }

        protected virtual void Start()
        {
            SetUpNotifier();
            ValidateNotifierNotNull();
            SetUpDrawers();
            SetUpSubscriptions();
            SetUpCaches();
        }

        private void SetUpCaches()
        {
            var currentTransform = transform;
            BeamCache = new LaserDrawerCache<LaserBeamController>(beamTemplate, currentTransform);
            _endParticlesCache = new LaserDrawerCache<LaserParticleController>(endParticlesTemplate, currentTransform);
        }

        protected virtual void SetUpSubscriptions()
        {
            Forwarder.LaserHitActor += OnLaserHitActor;
            Forwarder.LaserMiss += OnLaserMiss;
            Forwarder.LaserHitNonActor += OnLaserHitNonActor;
        }

        protected virtual void TearDownSubscriptions()
        {
            Forwarder.LaserHitActor -= OnLaserHitActor;
            Forwarder.LaserMiss -= OnLaserMiss;
            Forwarder.LaserHitNonActor -= OnLaserHitNonActor;
        }

        private void SetUpNotifier()
        {
            Forwarder = GetComponent<T>();
        }

        private void SetUpDrawers()
        {
            SetUpBeam();
            SetUpEndParticles();
        }

        private void SetUpBeam()
        {
            beamTemplate = new LaserComponentLoader<LaserBeamController>.Builder()
                .WithComponent(beamTemplate)
                .WithParent(transform)
                .WithPrefabPath(laserBeamAssetPath)
                .Build()
                .GetComponent();
        }

        private void SetUpEndParticles()
        {
            endParticlesTemplate = new LaserComponentLoader<LaserParticleController>.Builder()
                .WithComponent(endParticlesTemplate)
                .WithParent(beamTemplate.transform)
                .WithPrefabPath(endParticlesAssetPath)
                .Build()
                .GetComponent();
        }

        private void OnDestroy()
        {
            TearDownSubscriptions();
        }

        protected virtual void OnLaserHitNonActor(IQueryableLaserForwarder sender, LaserHit outgoingHit)
        {
            DrawLaserBeamFromHit(outgoingHit);
            DrawEndParticlesFromHit(outgoingHit);
        }

        protected virtual void OnLaserHitActor(IQueryableLaserForwarder sender, LaserHit outgoingHit, IQueryableLaserReceiver receiver)
        {
            DrawLaserBeamFromHit(outgoingHit);
            StopEndParticles(outgoingHit.Emitter);
        }

        protected virtual void OnLaserMiss(IQueryableLaserForwarder sender, LaserDirection outgoingDirection)
        {
            DrawLaserBeamFromDirection(outgoingDirection);
            StopEndParticles(outgoingDirection.Emitter);
        }

        protected virtual void OnLaserHitCeased(IQueryableLaserReceiver sender, LaserEmitter laserEmitter)
        {
            BeamCache.LocateLaserAsset(laserEmitter).EraseBeam();
            StopEndParticles(laserEmitter);
        }

        protected virtual void OnHitByLaser(IQueryableLaserReceiver sender, LaserHit incomingHit)
        {
            // no-op as default behaviour
        }

        private void DrawEndParticlesFromHit(LaserHit outgoingHit)
        {
            _endParticlesCache.LocateLaserAsset(outgoingHit.Emitter).Play(outgoingHit, ColorRegistry.GetParticleColor(outgoingHit.Emitter.GetInstanceID()));
        }

        private void StopEndParticles(LaserEmitter emitter)
        {
            _endParticlesCache.LocateLaserAsset(emitter).Stop();
        }

        private void DrawLaserBeamFromHit(LaserHit outgoingHit)
        {
            BeamCache.LocateLaserAsset(outgoingHit.Emitter).DrawLaserBeam(_laserDtoTransformer.Transform(outgoingHit), outgoingHit.Emitter.GetInstanceID());
        }

        private void DrawLaserBeamFromDirection(LaserDirection outgoingDirection)
        {
            BeamCache.LocateLaserAsset(outgoingDirection.Emitter).DrawLaserBeam(_laserDtoTransformer.Transform(outgoingDirection), outgoingDirection.Emitter.GetInstanceID());
        }

        private void ValidateNotifierNotNull()
        {
            if (Forwarder != null)
            {
                return;
            }

            throw new MissingLaserAssetException($"Game Object {name}: laser notifier is missing.");
        }
    }
}