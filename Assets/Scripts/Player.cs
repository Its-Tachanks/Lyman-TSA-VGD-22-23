using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform feet;

    [Header("Movement")]
    [SerializeField, Range(1,10)] private float horizontalRotateSpeed;
    [SerializeField, Range(1,10)] private float verticalRotateSpeed;
    [SerializeField] private Vector2 pitchBounds;
    [SerializeField, Range(1,20)] private float moveSpeed;
    [SerializeField, Range(1, 20)] private float airSpeed;
    [SerializeField] private float shiftMultiplier;
    [SerializeField, Range(1,100)] private float acceleration;
    [SerializeField, Range(1,20)] private float drag;
    [SerializeField, Range(0, 20)] private float airDrag;
    private float yaw = 0f, pitch = 0f;

    private float MoveSpeed
    {
        get
        {
            float shiftMod = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? shiftMultiplier : 1;

            if (isGrounded) return moveSpeed * shiftMod;
            return airSpeed * shiftMod;
        }
    }

    private float Drag
    {
        get
        {
            if (isGrounded) return drag;
            return airDrag;
        }
    }

    private ISelectable currentObject;

    [Header("Jumping")]
    [SerializeField, Range(0.01f, 1)] private float feetRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private bool readyToJump = true;
    [SerializeField, Range(0.01f, 1)] private float jumpCooldown;

    [SerializeField, Range(1, 100)] private float jumpForce;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (cam == null) cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateRotation();

        Move();
        ClampSpeed();

        Raycast();
        if (Input.GetMouseButton(0)) SelectObject();

        CheckForGround();

        if (Input.GetKey(KeyCode.Space)) Jump();

        rb.drag = Drag;      
    }

    private void Move()
    {
        int forward = 0, right = 0;
        if (Input.GetKey(KeyCode.W)) forward++;
        if (Input.GetKey(KeyCode.S)) forward--;
        if (Input.GetKey(KeyCode.D)) right++;
        if (Input.GetKey(KeyCode.A)) right--;

        Vector3 movementDir = transform.forward * forward + transform.right * right;
        rb.AddForce(movementDir.normalized * MoveSpeed * acceleration, ForceMode.Force);
    }

    private void ClampSpeed()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (vel.magnitude > MoveSpeed)
        {
            Vector3 limVel = vel.normalized * MoveSpeed;
            rb.velocity = new Vector3(limVel.x, rb.velocity.y, limVel.z);
        }
    }

    private void UpdateRotation()
    {
        yaw += horizontalRotateSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalRotateSpeed * Input.GetAxis("Mouse Y");

        cam.transform.localEulerAngles = new Vector3(Mathf.Clamp(pitch, pitchBounds.x, pitchBounds.y), 0f, 0f);
        transform.eulerAngles = new Vector3(0f, yaw, 0f);
    }

    void Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            ISelectable select;
            if (hit.transform.gameObject.TryGetComponent<ISelectable>(out select))
            {
                currentObject = select;
                currentObject.IsHovered = true;
            }
            else
            {
                UnHover();
            }
        }
        else
        {
            UnHover();
        }
    }

    void UnHover()
    {
        if (currentObject == null) return;

        currentObject.IsHovered = false;
        currentObject.IsSelected = false;
        currentObject = null;
    }

    void SelectObject()
    {
        if (currentObject == null) return;

        currentObject.IsSelected = !currentObject.IsSelected;
    }

    void CheckForGround()
    {
        Collider[] collisions = Physics.OverlapSphere(feet.position, feetRadius, groundLayer);
        isGrounded = collisions.Length > 0;
    }

    void Jump()
    {
        if (!isGrounded || !readyToJump) return;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        readyToJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);

    }

    void ResetJump()
    {
        readyToJump = true;
    }
}
