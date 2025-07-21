using LaserAssetPackage.Scripts.Laser.Drawing.Controller;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    /// <summary>
    /// Verifies that the built-in laser particle prefabs are present and have the necessary components.
    /// </summary>
    public class LaserParticlePrefabTest
    {
        private const string LaserParticlesResourcesPath = "Prefabs/Particles/";
        
        [Test]
        public void ActivationParticles_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var particles = Object.Instantiate(Resources.Load(LaserParticlesResourcesPath + "activation_particles")) as GameObject;
            
            // expect
            Assert.That(particles, "activation particles missing from prefabs.");
            Assert.IsNotNull(particles.GetComponent<LaserParticleController>());
            
            // check if there's at least one particle system
            Assert.IsNotNull(particles.GetComponentInChildren<ParticleSystem>());
        }
        
        [Test]
        public void ActivationParticles2_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var particles = Object.Instantiate(Resources.Load(LaserParticlesResourcesPath + "activation_particles_2")) as GameObject;
            
            // expect
            Assert.That(particles, "activation particles 2 missing from prefabs.");
            Assert.IsNotNull(particles.GetComponent<LaserParticleController>());
            
            // check if there's at least one particle system
            Assert.IsNotNull(particles.GetComponentInChildren<ParticleSystem>());
        }
        
        [Test]
        public void ActivationParticles3_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var particles = Object.Instantiate(Resources.Load(LaserParticlesResourcesPath + "activation_particles_3")) as GameObject;
            
            // expect
            Assert.That(particles, "activation particles 3 missing from prefabs.");
            Assert.IsNotNull(particles.GetComponent<LaserParticleController>());
            
            // check if there's at least one particle system
            Assert.IsNotNull(particles.GetComponentInChildren<ParticleSystem>());
        }
        
        [Test]
        public void HitParticles_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var particles = Object.Instantiate(Resources.Load(LaserParticlesResourcesPath + "hit_particles")) as GameObject;
            
            // expect
            Assert.That(particles, "hit particles missing from prefabs.");
            Assert.IsNotNull(particles.GetComponent<LaserParticleController>());
            
            // check if there's at least one particle system
            Assert.IsNotNull(particles.GetComponentInChildren<ParticleSystem>());
        }
        
        [Test]
        public void MirrorParticles_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var particles = Object.Instantiate(Resources.Load(LaserParticlesResourcesPath + "mirror_particles")) as GameObject;
            
            // expect
            Assert.That(particles, "mirror particles missing from prefabs.");
            Assert.IsNotNull(particles.GetComponent<LaserParticleController>());
            
            // check if there's at least one particle system
            Assert.IsNotNull(particles.GetComponentInChildren<ParticleSystem>());
        }
        
        [Test]
        public void LaserBeam_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var particles = Object.Instantiate(Resources.Load(LaserParticlesResourcesPath + "laser_beam")) as GameObject;
            
            // expect
            Assert.That(particles, "laser beam missing from prefabs.");
            Assert.IsNotNull(particles.GetComponent<LaserBeamController>());
           
            // check for components
            Assert.IsNotNull(particles.GetComponent<LineRenderer>());
            Assert.IsNotNull(particles.GetComponent<LineRenderer>().sharedMaterial);
        }
    }
}