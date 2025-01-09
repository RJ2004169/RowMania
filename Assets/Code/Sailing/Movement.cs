using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
   [Header("Movement Settings")]
   [SerializeField] private float maxSpeed = 10f;
   [SerializeField] private float acceleration = 5f;
   [SerializeField] private float deceleration = 2f;
   [SerializeField] private float rotationSpeed = 100f;
   [SerializeField] private float minSpeedForRotation = 50f; // Minimum speed needed to rotate
   
   private Vector2 moveInput;
   private float currentSpeed;
   private Rigidbody rb;
   private float rotationVelocity;
   private float targetRotation;

   private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject.");
            enabled = false;
        }
    }

   public void OnMove(InputValue value)
   {
       moveInput = value.Get<Vector2>();
   }

   private void FixedUpdate()
   {
       float targetSpeed = moveInput.y * maxSpeed;
       currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, 
           (moveInput.y != 0 ? acceleration : deceleration) * Time.fixedDeltaTime);

       Vector3 movement = transform.forward * currentSpeed;
       rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

       // Smoothly rotate based on speed
       float rotationFactor = Mathf.InverseLerp(0, minSpeedForRotation, Mathf.Abs(currentSpeed));
       if (rotationFactor > 0)
       {
           targetRotation += moveInput.x * rotationSpeed * Time.fixedDeltaTime * rotationFactor;
           float smoothRotation = Mathf.SmoothDampAngle(rb.rotation.eulerAngles.y, targetRotation, ref rotationVelocity, 0.1f);
           rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles.x, smoothRotation, rb.rotation.eulerAngles.z));
       }
   }
}