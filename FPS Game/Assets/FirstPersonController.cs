using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] LayerMask floorLayer;
    public Rigidbody rb;
    private Vector3 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    float groundRayLength = 0.2f;
    Vector3 groundRayStart = new Vector3(0, -1f, 0);

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, moveInput.z * speed);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector3>();
    }

    bool IsGrounded()
    {
        Ray groundCheckRay = new Ray(this.transform.position + groundRayStart, Vector3.down);
        Debug.DrawRay(this.transform.position + groundRayStart, Vector3.down, Color.red, 10f);
        return Physics.Raycast(groundCheckRay, groundRayLength, floorLayer);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
        }
    }
}
