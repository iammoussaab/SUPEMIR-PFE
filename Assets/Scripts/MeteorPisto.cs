using UnityEngine;
using UnityEngine.XR;


public class MeteorPisto : MonoBehaviour
{

    public ParticleSystem shootingEffect; // Reference to the particle system for shooting effect
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => startShooting());
        grabInteractable.deactivated.AddListener(x => stopShooting());
    }

    public void startShooting()
    {
        shootingEffect.Play(); // Play the shooting effect particle system
        // Logic to start shooting
        Debug.Log("Shooting started");
    }

    public void stopShooting()
    {
        shootingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Correct enum value
        // Logic to stop shooting
        Debug.Log("Shooting stopped");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
