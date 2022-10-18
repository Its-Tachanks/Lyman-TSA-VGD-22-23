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
    [SerializeField, Range(1, 100)] private float airAcceleration;
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

    private float Acceleration
    {
        get
        {
            if (isGrounded) return acceleration;
            return airAcceleration;
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

    [Header("Selection")]
    private ISelectable currentObject;
    private ISelectable holdingObject;
    [SerializeField] private GameObject hand;
    [SerializeField, Range(0, 100)] private float selectionRange;
    [SerializeField, Range(1, 100)] private float pickupSpeed;

    [Header("Jumping")]
    [SerializeField, Range(0.01f, 1)] private float feetRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private bool readyToJump = true;
    [SerializeField, Range(0.01f, 1)] private float jumpCooldown;

    [SerializeField, Range(1, 100)] private float jumpForce;

    public static Player instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple Players in Scene");
        }
    }

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
        if (Input.GetMouseButtonDown(0))
        {
            if (holdingObject == null)
            {
                SelectObject();
            }
            else
            {
                UnSelectObject();
            }
        }

        if (holdingObject == null) hand.GetComponent<Joint>().connectedBody = null;

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
        rb.AddForce(movementDir.normalized * MoveSpeed * Acceleration, ForceMode.Force);
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
        if (Physics.Raycast(ray, out hit, selectionRange))
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
        if (currentObject == null || holdingObject != null) return;

        currentObject.IsHovered = false;
        if (currentObject.IsSelected) currentObject.IsSelected = false;
        currentObject = null;
    }

    void SelectObject()
    {
        if (currentObject == null) return;

        currentObject.IsSelected = !currentObject.IsSelected;
    }

    void UnSelectObject()
    {
        (holdingObject as MonoBehaviour).transform.parent = null;
        holdingObject.IsSelected = false;
        hand.GetComponent<Joint>().connectedBody = null;
        MyFunctions.IgnoreAllCollisions(transform, (holdingObject as MonoBehaviour).transform, false);
        holdingObject = null;
    }


    public void Pickup(ISelectable obj)
    {
        holdingObject = obj;
        MonoBehaviour objM = obj as MonoBehaviour;
        objM.transform.parent = hand.transform;
        StartCoroutine(MoveObjectToOrigin(objM.transform));
        MyFunctions.IgnoreAllCollisions(transform, objM.transform);
    }

    IEnumerator MoveObjectToOrigin(Transform obj)
    {
        Rigidbody oRb = obj.gameObject.GetComponent<Rigidbody>();
        oRb.isKinematic = true;
        while (obj.localPosition.magnitude > 0.05f)
        {
            obj.localPosition = Vector3.MoveTowards(obj.localPosition, Vector3.zero, pickupSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        obj.localPosition = Vector3.zero;
        oRb.isKinematic = false;
        hand.GetComponent<Joint>().connectedBody = oRb;
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
