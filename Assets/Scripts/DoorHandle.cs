using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorInteraction : MonoBehaviour
{
    public Transform handle; // Assign this in the inspector
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool hasKey = false;

    private bool isOpening = false;

    void Start()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable = handle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.activated.AddListener(OnHandleActivated);
        }
        else
        {
            Debug.LogWarning("No XRBaseInteractable on handle!");
        }
    }

    void OnHandleActivated(ActivateEventArgs args)
    {
        Debug.Log("Handle activated!");

        if (hasKey)
        {
            Debug.Log("Door opening...");
            isOpening = true;
        }
        else
        {
            Debug.Log("Door locked. No key.");
        }
    }

    void Update()
    {
        if (isOpening)
        {
            float step = openSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0, openAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, step);
        }
    }
}
