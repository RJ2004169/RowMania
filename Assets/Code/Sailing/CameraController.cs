using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target; // The boat's transform
    public float distance = 5.0f;
    public float height = 2.0f;
    public float rotationSpeed = 2.0f;

    private Vector2 _lookInput;

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // Rotate the camera based on mouse input
        Quaternion rotation = Quaternion.Euler(-_lookInput.y * rotationSpeed, _lookInput.x * rotationSpeed, 0);
        desiredPosition = target.position + rotation * (desiredPosition - target.position);

        Vector3 currentPosition = transform.position;
        Vector3 smoothPosition = Vector3.SmoothDamp(currentPosition, desiredPosition, ref _velocity, 0.2f);
        transform.position = smoothPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }

    private Vector3 _velocity;
}
