using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] LayerMask floorLayer;
    public Rigidbody rb;
    private Vector3 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] Transform gC;
    Shooting shooting;
    private bool sprinting;
    

    [Header("Camera Look")]
    [SerializeField] Transform cam;
    public float sens = 200f;
    bool invertY = false;
    private float maxPitch = 89f;
    private float minPitch = -89f;
    
    private Vector2 lookInput = Vector2.zero;
    public float pitch;
    private float yaw;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;

        if (cam != null)
            pitch = cam.localEulerAngles.x;

        if (pitch > 180f) pitch -= 360f;

        shooting = GetComponentInChildren<Shooting>();
        
    }

    private void Update()
    {
        ApplyMovement();
        ApplyLook();

        if(sprinting == true)
        {
            speed = 10;
        }
        else if(sprinting == false)
        {
            speed = 5;
        }
    }

    void ApplyMovement()
    {
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.z + right * moveInput.x;

        rb.linearVelocity = new Vector3(moveDir.x * speed,
                                        rb.linearVelocity.y,
                                        moveDir.z * speed);
    }

    void ApplyLook()
    {
        if (cam == null) return;

        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        yaw += mouseX * sens * Time.deltaTime;
        pitch += (invertY ? mouseY : -mouseY) * sens * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 m = context.ReadValue<Vector2>();
        moveInput = new Vector3(m.x, 0f, m.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
        }
    }

    bool IsGrounded()
    {
        float radius = 0.5f;
        Vector3 point1 = gC.position + Vector3.up * 0.1f;
        Vector3 point2 = gC.position + Vector3.down * 0.1f;

        return Physics.OverlapCapsule(point1, point2, radius, floorLayer).Length > 0;
    }

    public void onCrouch(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            this.transform.localScale = new Vector3(1, 0.5f, 1);
            cam.transform.localScale = new Vector3(1, 2, 1);
            speed = 1.5f;

        }
        if(context.canceled)
        {
            transform.localScale = new Vector3(1, 1, 1);
            cam.transform.localScale = new Vector3(1, 1, 1);
            speed = 5;
        }
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnWeaponChange(InputAction.CallbackContext context)
    {
        if(context.performed && shooting.gun == "pistol")
        {
            shooting.pistol.SetActive(false);
            shooting.shotgun.SetActive(true);
        }

        else if (context.performed && shooting.gun == "shotgun")
        {
            shooting.shotgun.SetActive(false);
            shooting.ak.SetActive(true);
        }
        else if (context.performed && shooting.gun == "ak")
        {
            shooting.ak.SetActive(false);
            shooting.pistol.SetActive(true);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && sprinting == false)
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

    }
}
