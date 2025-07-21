using System.Collections.Generic;
using System.Linq;
using LaserAssetPackage.Scripts.Laser.Drawing.Coloring;
using LaserAssetPackage.Scripts.Laser.Dto;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing.Controller
{
    /// <summary>
    /// Draws particles for the laser system. Includes an optional light that is switched on or off (if exists) when the
    /// particle effects are active.
    /// </summary>
    public class LaserParticleController : MonoBehaviour
    {
        private const float Epsilon = 0.04f;

        private IList<ParticleSystem> _particles;
        private IList<ParticleSystem> _colorableParticles;
        private IList<Material> _particleMaterials;
        private Light _optionalLight;
        private static readonly int ParticleMainColor = Shader.PropertyToID("_Color");

        private void Awake()
        {
            SetUpParticles();
            SetUpLight();
            LightOff();
        }

        /// <summary>
        /// Play the particle system at the hit point, pointing to its origin.
        /// </summary>
        /// <param name="hit">the hit data from the laser hit</param>
        /// <param name="tint">the color of the particles and the light</param>
        public void Play(LaserHit hit, Color tint)
        {
            SetParticleColors(tint);
            SetParticleSystemTransforms(hit);
            PlayParticleEffect();
            LightOn(tint);
        }

        /// <summary>
        /// Play the particle system at the origin of this GameObject.
        /// </summary>
        /// <param name="tint">the color of the particles and the light</param>
        public void Play(Color tint)
        {
            SetParticleColors(tint);
            PlayParticleEffect();
            LightOn(tint);
        }

        private void SetParticleColors(Color tint)
        {
            SetStartColor(tint);
            SetMaterialColorWithoutIntensity(tint);
        }

        private void SetMaterialColorWithoutIntensity(Color tint)
        {
            foreach (var material in _particleMaterials)
            {
                material.SetColor(ParticleMainColor, tint);
            }
        }

        private void SetStartColor(Color tint)
        {
            foreach (var particle in _colorableParticles)
            {
                var main = particle.main;
                main.startColor = tint;
            }
        }

        private void PlayParticleEffect()
        {
            foreach (var particle in _particles)
            {
                if (!particle.isPlaying)
                {
                    particle.Play();
                }
            }
        }

        private void SetParticleSystemTransforms(LaserHit hit)
        {
            transform.position = TranslateForward(hit);
            transform.LookAt(hit.Origin);
        }

        private Vector3 TranslateForward(LaserHit hit)
        {
            // moving the particles forward so they aren't blocked halfway by the mesh they hit
            return hit.Hit.point + hit.Hit.normal.normalized * Epsilon;
        }

        /// <summary>
        /// Stop playing the particles and light immediately.
        /// </summary>
        public void Stop()
        {
            foreach (var particle in _particles)
            {
                particle.Stop
                (
                    withChildren: true,
                    ParticleSystemStopBehavior.StopEmitting
                );
            }

            LightOff();
        }

        private void SetUpParticles()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();

            if (_particles == null)
            {
                _particles = new List<ParticleSystem>();
                UnityEngine.Debug.LogWarning($"No particle systems found on object {name}. No particles will be emitted on laser hit. ");
                return;
            }

            SetUpColorableParticles();
            SetUpParticleMaterials();
        }

        private void SetUpParticleMaterials()
        {
            _particleMaterials = _colorableParticles
                .Select(p => p.GetComponent<Renderer>().material)
                .ToList();
        }

        private void SetUpColorableParticles()
        {
            _colorableParticles = _particles
                .Where(p => !p.GetComponent<IgnoreColoring>())
                .ToList();
        }

        private void LightOn(Color tint)
        {
            if (!_optionalLight)
            {
                return;
            }

            _optionalLight.color = tint;
            _optionalLight.enabled = true;
        }

        private void LightOff()
        {
            if (!_optionalLight)
            {
                return;
            }

            _optionalLight.enabled = false;
        }

        private void SetUpLight()
        {
            _optionalLight = GetComponentInChildren<Light>();
        }
    }
}