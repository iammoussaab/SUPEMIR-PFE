using LaserAssetPackage.Scripts.Laser.Drawing;
using LaserAssetPackage.Scripts.Laser.Logic;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    /// <summary>
    /// Verifies that the built-in prefabs have the components that they need.
    /// </summary>
    public class LaserActorPrefabTest
    {
        private const string LaserActorResourcesPath = "Prefabs/LaserActors/";
        private const string LaserParticlesResourcesPath = "Prefabs/Particles/";
        private const string LaserBeamResourcePath = LaserParticlesResourcesPath + "laser_beam";
        private const string EndParticlesResourcesPath = LaserParticlesResourcesPath + "hit_particles";
        private const string EmitterHitParticlesPath = LaserParticlesResourcesPath + "activation_particles_3";
        private const string MirrorHitParticlesPath = LaserParticlesResourcesPath + "mirror_particles";
        private const string ActivationParticlesPath = LaserParticlesResourcesPath + "activation_particles";
        private const string BlockingReceiverActivationParticlesPath = LaserParticlesResourcesPath + "activation_particles_2";

        [Test]
        public void EmitterCubePrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var emitter = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "emitter_cube")) as GameObject;

            // expect
            Assert.That(emitter, "a prefab named emitter_cube comes with the asset package and is missing. if you make an emitter of your own give it another name.");
            Assert.IsNotNull(emitter.GetComponent<Rigidbody>());
            Assert.IsNotNull(emitter.GetComponent<Collider>());

            // checking scripts
            Assert.IsNotNull(emitter.GetComponent<LaserActorRoot>());
            
            var emitterScripts = emitter.GetComponentsInChildren<LaserEmitter>();

            Assert.IsNotNull(emitterScripts);
            Assert.That(1 == emitterScripts.Length, "There should be exactly one Laser Emitter script on the emitter.");

            var drawerScripts = emitter.GetComponentsInChildren<EmitterLaserDrawer>();

            Assert.IsNotNull(drawerScripts);
            Assert.That(1 == drawerScripts.Length, "There should be exactly one Laser Drawer script on the emitter.");

            // checking the drawer's referenced prefabs
            EmitterLaserDrawer drawerScript = drawerScripts[0];

            Assert.AreEqual(LaserBeamResourcePath, drawerScript.laserBeamAssetPath);
            Assert.AreEqual(EndParticlesResourcesPath, drawerScript.endParticlesAssetPath);
            Assert.AreEqual(EmitterHitParticlesPath, drawerScript.activationParticlesPath);
            
            // checking the activation particles origin
            Assert.IsNotNull(drawerScript.transform.Find("laser_origin"));
        }

        [Test]
        public void MirrorPrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var mirror = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "mirror")) as GameObject;
            
            // expect
            Assert.That(mirror, "a prefab named mirror comes with the asset package and is missing. if you make a mirror of your own give it another name.");
            Assert.IsNotNull(mirror.GetComponent<Rigidbody>());
            Assert.IsNotNull(mirror.GetComponent<Collider>());
            
            // checking scripts
            Assert.IsNotNull(mirror.GetComponent<LaserActorRoot>());

            var mirrorScript = mirror.GetComponent<LaserMirror>();
            Assert.IsNotNull(mirrorScript);

            var drawerScript = mirror.GetComponent<MultiTargetLaserDrawer>();
            Assert.IsNotNull(drawerScript);
            
            // checking the drawer's referenced prefabs
            Assert.AreEqual(LaserBeamResourcePath, drawerScript.laserBeamAssetPath);
            Assert.AreEqual(EndParticlesResourcesPath, drawerScript.endParticlesAssetPath);
            Assert.AreEqual(MirrorHitParticlesPath, drawerScript.mirrorParticlesPath);
        }

        [Test]
        public void NonBlockingReceiverPrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var nonblockingReceiver = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "nonblocking_receiver")) as GameObject;
            
            // expect
            Assert.That(nonblockingReceiver, "a prefab named nonblocking_receiver comes with the asset package and is missing. if you make a nonblocking receiver of your own give it another name.");
            Assert.IsNotNull(nonblockingReceiver.GetComponent<Rigidbody>());
            Assert.IsNotNull(nonblockingReceiver.GetComponent<Collider>());
            
            // checking scripts
            Assert.IsNotNull(nonblockingReceiver.GetComponent<LaserActorRoot>());

            var nbrScript = nonblockingReceiver.GetComponent<NonBlockingLaserReceiver>();
            Assert.IsNotNull(nbrScript);

            var drawerScript = nonblockingReceiver.GetComponent<SingleTargetLaserDrawer>();
            Assert.IsNotNull(drawerScript);
            
            // checking the drawer's referenced prefabs
            Assert.AreEqual(LaserBeamResourcePath, drawerScript.laserBeamAssetPath);
            Assert.AreEqual(EndParticlesResourcesPath, drawerScript.endParticlesAssetPath);
            Assert.AreEqual(ActivationParticlesPath, drawerScript.activationParticlesPath);
        }

        [Test]
        public void BlockingReceiverPrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var receiver = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "receiver_cube")) as GameObject;

            // expect
            Assert.That(receiver, "a prefab named receiver_cube comes with the asset package and is missing. if you make an emitter of your own give it another name.");
            Assert.IsNotNull(receiver.GetComponent<Rigidbody>());
            Assert.IsNotNull(receiver.GetComponent<Collider>());

            // checking scripts
            Assert.IsNotNull(receiver.GetComponent<LaserActorRoot>());
            
            var receiverScripts = receiver.GetComponentsInChildren<BlockingLaserReceiver>();

            Assert.IsNotNull(receiverScripts);
            Assert.That(1 == receiverScripts.Length, "There should be exactly one Receiver script on the receiver..");

            var drawerScripts = receiver.GetComponentsInChildren<NonEmittingLaserDrawer>();

            Assert.IsNotNull(drawerScripts);
            Assert.That(1 == drawerScripts.Length, "There should be exactly one Laser Drawer script on the receiver.");

            // checking the drawer's referenced prefabs
            NonEmittingLaserDrawer drawerScript = drawerScripts[0];
            
            Assert.AreEqual(BlockingReceiverActivationParticlesPath, drawerScript.activationParticlesAssetPath);
            
            // checking the activation particles origin
            Assert.IsNotNull(drawerScript.transform.Find("activation_particles_position"));
        }

        [Test]
        public void RepeaterPrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var repeater = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "repeater_cube")) as GameObject;
            
            // expect
            Assert.That(repeater, "a prefab named repeater_cube comes with the asset package and is missing. if you make a nonblocking receiver of your own give it another name.");
            Assert.IsNotNull(repeater.GetComponent<Rigidbody>());
            Assert.IsNotNull(repeater.GetComponent<Collider>());
            
            // checking scripts
            Assert.IsNotNull(repeater.GetComponent<LaserActorRoot>());

            var repeaterScript = repeater.GetComponent<LaserRepeater>();
            Assert.IsNotNull(repeaterScript);

            var drawerScript = repeater.GetComponent<SingleTargetLaserDrawer>();
            Assert.IsNotNull(drawerScript);
            
            // checking the drawer's referenced prefabs
            Assert.AreEqual(LaserBeamResourcePath, drawerScript.laserBeamAssetPath);
            Assert.AreEqual(EndParticlesResourcesPath, drawerScript.endParticlesAssetPath);
            Assert.AreEqual(ActivationParticlesPath, drawerScript.activationParticlesPath);
        }

        [Test]
        public void StandaloneEmitterPrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var emitter = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "standalone_emitter")) as GameObject;

            // expect
            Assert.That(emitter, "a prefab named standalone_emitter comes with the asset package and is missing. if you make an emitter of your own give it another name.");
            Assert.IsNotNull(emitter.GetComponent<Collider>());

            // checking scripts
            Assert.IsNotNull(emitter.GetComponent<LaserActorRoot>());
            
            var emitterScript = emitter.GetComponent<LaserEmitter>();
            Assert.IsNotNull(emitterScript);
            
            var drawerScript = emitter.GetComponent<EmitterLaserDrawer>();
            Assert.IsNotNull(drawerScript);
            
            // checking the drawer's referenced prefabs

            Assert.AreEqual(LaserBeamResourcePath, drawerScript.laserBeamAssetPath);
            Assert.AreEqual(EndParticlesResourcesPath, drawerScript.endParticlesAssetPath);
            Assert.AreEqual(EmitterHitParticlesPath, drawerScript.activationParticlesPath);
            
            // checking the activation particles origin
            Assert.IsNotNull(drawerScript.transform.Find("laser_origin"));
        }

        [Test]
        public void StandaloneReceiverPrefab_ShouldHave_AllTheNecessaryComponents()
        {
            // given
            var receiver = Object.Instantiate(Resources.Load(LaserActorResourcesPath + "standalone_blocking_receiver")) as GameObject;

            // expect
            Assert.That(receiver, "a prefab named standalone_blocking_receiver comes with the asset package and is missing. if you make an emitter of your own give it another name.");
            Assert.IsNotNull(receiver.GetComponent<Collider>());

            // checking scripts
            Assert.IsNotNull(receiver.GetComponent<LaserActorRoot>());
            
            var receiverScript = receiver.GetComponent<BlockingLaserReceiver>();
            Assert.IsNotNull(receiverScript);

            var drawerScript = receiver.GetComponent<NonEmittingLaserDrawer>();
            Assert.IsNotNull(drawerScript);

            // checking the drawer's referenced prefabs
            Assert.AreEqual(BlockingReceiverActivationParticlesPath, drawerScript.activationParticlesAssetPath);
            
            // checking the activation particles origin
            Assert.IsNotNull(drawerScript.transform.Find("activation_particles_position"));
        }
    }
}