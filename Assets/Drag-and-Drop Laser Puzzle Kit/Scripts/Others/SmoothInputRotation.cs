using UnityEngine;

namespace MoonBika
{
    public class KeyboardRotator : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 90f; // Degrees per second
        [SerializeField] private KeyCode rotateLeftKey = KeyCode.Q;
        [SerializeField] private KeyCode rotateRightKey = KeyCode.E;
        [SerializeField] private KeyCode resetRotationKey = KeyCode.R;

        private Quaternion initialRotation;
        private float currentRotation = 0f;

        private void Start()
        {
            // Store the initial rotation as reference
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            HandleRotationInput();
        }

        private void HandleRotationInput()
        {
            // Rotate left
            if (Input.GetKey(rotateLeftKey))
            {
                currentRotation += rotationSpeed * Time.deltaTime;
                ApplyRotation();
            }

            // Rotate right
            if (Input.GetKey(rotateRightKey))
            {
                currentRotation -= rotationSpeed * Time.deltaTime;
                ApplyRotation();
            }

            // Reset to initial rotation
            if (Input.GetKeyDown(resetRotationKey))
            {
                currentRotation = 0f;
                transform.rotation = initialRotation;
            }
        }

        private void ApplyRotation()
        {
            // Apply rotation relative to initial rotation
            transform.rotation = initialRotation * Quaternion.Euler(0, currentRotation, 0);
        }
    }
}