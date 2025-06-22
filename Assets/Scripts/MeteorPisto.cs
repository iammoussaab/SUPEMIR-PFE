using UnityEngine;
using UnityEngine.XR;


public class MeteorPisto : MonoBehaviour
{

    public ParticleSystem shootingEffect; // Reference to the particle system for shooting effect

    public LayerMask layerMask; // Layer mask to filter raycast hits
    public Transform shootsource; // Transform from where the shooting effect originates
    public float shootRange = 10; // Range of the shooting effect

    private bool rayActive = false; // Flag to check if raycasting is active



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
        rayActive = true; // Set rayActive to true to indicate shooting is active
        Debug.Log("Shooting started");
    }

    public void stopShooting()
    {
        shootingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Correct enum value
        rayActive = false; // Set rayActive to false to indicate shooting is stopped
        // Logic to stop shooting
        Debug.Log("Shooting stopped");
    }

    // Update is called once per frame
    void Update()
    {
        if (rayActive)
            RaycastCheck(); // Check for raycast hits every frame
    }

    void RaycastCheck()
    {
        RaycastHit hit;

        bool hasHit = Physics.Raycast(shootsource.position, shootsource.forward, out hit, shootRange, layerMask);
        if(hasHit)
        {
            hit.transform.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver); // Send a message to the hit object to break it
        }
    }
}
