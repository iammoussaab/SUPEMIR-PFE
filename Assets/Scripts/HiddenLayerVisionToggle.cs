using UnityEngine;
using UnityEngine.InputSystem;

public class HiddenLayerVisionToggle : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera vrCamera;

    [Header("Input Action")]
    [SerializeField] private InputActionReference toggleVisionAction; // Assign 'ToggleVision' action here

    [Header("Layer to Reveal")]
    [SerializeField] private string hiddenLayerName = "HiddenObjects";

    private int hiddenLayerMask;
    private bool isVisionOn = false;

    void Start()
    {
        if (vrCamera == null)
            vrCamera = Camera.main;

        if (toggleVisionAction == null)
        {
            Debug.LogError("ToggleVision InputActionReference is not assigned!");
            enabled = false;
            return;
        }

        int layer = LayerMask.NameToLayer(hiddenLayerName);
        if (layer == -1)
        {
            Debug.LogError($"Layer '{hiddenLayerName}' does not exist!");
            enabled = false;
            return;
        }
        hiddenLayerMask = 1 << layer;

        toggleVisionAction.action.performed += OnToggleVision;
        toggleVisionAction.action.Enable();
    }

    void OnDestroy()
    {
        if (toggleVisionAction != null)
            toggleVisionAction.action.performed -= OnToggleVision;
    }

    private void OnToggleVision(InputAction.CallbackContext ctx)
    {
        if (isVisionOn)
            vrCamera.cullingMask &= ~hiddenLayerMask; // Hide layer
        else
            vrCamera.cullingMask |= hiddenLayerMask;  // Show layer

        isVisionOn = !isVisionOn;
    }
}
