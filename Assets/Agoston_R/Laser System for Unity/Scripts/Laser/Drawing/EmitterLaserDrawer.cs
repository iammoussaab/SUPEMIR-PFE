using LaserAssetPackage.Scripts.Laser.Drawing.Coloring;
using LaserAssetPackage.Scripts.Laser.Drawing.Controller;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Exceptions;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Scripts.Laser.ResourceLoading;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing
{
    /// <summary>
    /// Draws beams and activation particle effects for a <see cref="LaserEmitter"/>.
    /// </summary>
    public class EmitterLaserDrawer : ForwarderLaserDrawer<LaserEmitter>
    {
        [Tooltip("Resources path to the particle system that plays when the emitter is active.")] 
        public string activationParticlesPath;

        [Tooltip("Controller from a particle system that plays when the emitter is active. Exposed for drag n drop.")] 
        public LaserParticleController activationParticlesTemplate;

        [Tooltip("Transform that can be used to define an exact location on the emitter for the activation particles.")] 
        public Transform activationParticlesOrigin;

        [Tooltip("Color multiplier for the laser beam.")]
        [ColorUsage(true, true)]
        public Color laserColor;

        [Tooltip("Color multiplier for the laser particle effects.")]
        [ColorUsage(true, true)]
        public Color particleColor;

        private LaserBeamController _beam;
        private readonly LaserColorRegistry _colorRegistry = LaserColorRegistry.Instance;

        protected override void Start()
        {
            base.Start();
            SetUpLaserBeam();
            SetUpActivationParticles();
            AddColorToRegistry();
        }

        /// <summary>
        /// Updates the beam color for this emitter drawer.
        /// </summary>
        /// <param name="color">new color for the beam</param>
        public void UpdateLaserColor(Color color)
        {
            laserColor = color;
            _colorRegistry.RegisterBeamColor(Forwarder, laserColor);
        }

        /// <summary>
        /// Updates the particle color for this emitter drawer.
        /// </summary>
        /// <param name="color">new color for the particles</param>
        public void UpdateParticleColor(Color color)
        {
            particleColor = color;
            _colorRegistry.RegisterParticleColor(Forwarder, particleColor);
        }

        private void AddColorToRegistry()
        {
            _colorRegistry.RegisterBeamColor(Forwarder, laserColor);
            _colorRegistry.RegisterParticleColor(Forwarder, particleColor);
        }

        private void SetUpLaserBeam()
        {
            _beam = GetComponentInChildren<LaserBeamController>();
            if (!_beam)
            {
                throw new MissingLaserAssetException($"Laser beam controller missing from emitter drawer {name}");
            }
        }

        protected override void SetUpSubscriptions()
        {
            base.SetUpSubscriptions();
            Forwarder.EmitterDeactivated += OnEmitterDeactivated;
        }

        protected override void TearDownSubscriptions()
        {
            base.TearDownSubscriptions();
            Forwarder.EmitterDeactivated -= OnEmitterDeactivated;
        }

        protected override void OnLaserHitActor(IQueryableLaserForwarder sender, LaserHit laserHit, IQueryableLaserReceiver receiver)
        {
            base.OnLaserHitActor(sender, laserHit, receiver);
            activationParticlesTemplate.Play(ExtractParticlesColor(laserHit.Emitter));
        }

        protected override void OnLaserMiss(IQueryableLaserForwarder sender, LaserDirection laserDirection)
        {
            base.OnLaserMiss(sender, laserDirection);
            activationParticlesTemplate.Play(ExtractParticlesColor(laserDirection.Emitter));
        }

        protected override void OnLaserHitNonActor(IQueryableLaserForwarder sender, LaserHit laserHit)
        {
            base.OnLaserHitNonActor(sender, laserHit);
            activationParticlesTemplate.Play(ExtractParticlesColor(laserHit.Emitter));
        }

        private void OnEmitterDeactivated(IQueryableLaserEmitter sender)
        {
            activationParticlesTemplate.Stop();
            _beam.EraseBeam();
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

        private Color ExtractParticlesColor(LaserEmitter emitter)
        {
            return ColorRegistry.GetParticleColor(emitter.GetInstanceID());
        }

        private Transform GetActivationParticlesOrigin()
        {
            return activationParticlesOrigin != null ? activationParticlesOrigin : transform;
        }
    }
}