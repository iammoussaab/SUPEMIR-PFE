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
    /// Handles the drawings for the <see cref="BlockingLaserReceiver"/>.
    /// </summary>
    public class NonEmittingLaserDrawer : MonoBehaviour
    {
        private IQueryableLaserReceiver _receiver;

        [Tooltip("Resources path for the particle system that plays when the receiver is hit by a laser.")]
        public string activationParticlesAssetPath;

        [Tooltip("Controller from the particle system that plays when the receiver is hit by a laser. Exposed for drag n drop.")]
        public LaserParticleController activationParticlesTemplate;

        [Tooltip("Transform to give an exact place for the activation particle system to play at. Exposed for drag n drop.")]
        public Transform activationParticlesOrigin;

        private LaserColorRegistry ColorRegistry => LaserColorRegistry.Instance;
        private readonly ParticleColorBlender _colorBlender = new ParticleColorBlender();
        private Color _particleColor = Color.white;

        private void Start()
        {
            SetUpNotifier();
            ValidateNotifierNotNull();
            SetUpSubscriptions();
            SetUpActivationParticles();
        }

        private void OnHitByLaser(IQueryableLaserReceiver sender, LaserHit incomingHit)
        {
            activationParticlesTemplate.Play(_particleColor);
        }

        private void OnAllHitsCeased(IQueryableLaserReceiver sender)
        {
            activationParticlesTemplate.Stop();
        }

        private void SetUpActivationParticles()
        {
            activationParticlesTemplate = new LaserComponentLoader<LaserParticleController>.Builder()
                .WithComponent(activationParticlesTemplate)
                .WithParent(GetActivationParticlesOrigin())
                .WithPrefabPath(activationParticlesAssetPath)
                .Build()
                .GetComponent();
        }

        private Transform GetActivationParticlesOrigin()
        {
            return activationParticlesOrigin != null ? activationParticlesOrigin : transform;
        }

        private void SetUpSubscriptions()
        {
            _receiver.HitByLaser += OnHitByLaser;
            _receiver.OnNewEmitterReceived += OnNewEmitterReceived;
            _receiver.OnEmitterDetached += OnEmitterDetached;
            _receiver.AllLaserHitsCeased += OnAllHitsCeased;
        }

        private void OnDestroy()
        {
            _receiver.HitByLaser -= OnHitByLaser;
            _receiver.OnNewEmitterReceived -= OnNewEmitterReceived;
            _receiver.OnEmitterDetached -= OnEmitterDetached;
            _receiver.AllLaserHitsCeased -= OnAllHitsCeased;
        }

        private void OnNewEmitterReceived(IQueryableLaserReceiver sender, LaserHit laserHit)
        {
            int id = laserHit.Emitter.GetInstanceID();
            _particleColor = _colorBlender
                .AddEmitterWithColor(id, ColorRegistry.GetParticleColor(id))
                .CalculateColor();
        }

        private void OnEmitterDetached(IQueryableLaserReceiver sender, LaserEmitter laserEmitter)
        {
            int id = laserEmitter.GetInstanceID();
            _particleColor = _colorBlender
                .RemoveEmitter(id)
                .CalculateColor();
        }

        private void SetUpNotifier()
        {
            _receiver = GetComponent<IQueryableLaserReceiver>();
        }

        private void ValidateNotifierNotNull()
        {
            if (_receiver != null)
            {
                return;
            }

            throw new MissingLaserAssetException($"Game Object {name}: laser notifier is missing.");
        }
    }
}