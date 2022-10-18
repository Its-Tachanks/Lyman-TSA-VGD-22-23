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

    /*
     * These properties should be used instead of drag, moveSpeed, etc
     * They account for whether the object is in the air or not
     */
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
    [SerializeField] private GameObject hand;
    [SerializeField, Range(0, 100)] private float selectionRange;
    [SerializeField, Range(1, 100)] private float pickupSpeed;
    private ISelectable currentObject;
    private ISelectable holdingObject;

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
        //setting up static instance
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
        //assigning references if not done already
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (cam == null) cam = GetComponentInChildren<Camera>();

        //Cursor is gone
        Cursor.lockState = CursorLockMode.Locked;
    }

    /*Update is called every frame update
     *Code in here runs often
     */
    private void Update()
    {
        UpdateRotation();

        Move();
        ClampSpeed();

        Raycast();
        if (Input.GetMouseButtonDown(0)) //OnClick
        {
            if (holdingObject == null)
            {
                SelectObject();
            }
            else
            {
                UnSelectObject();
                if (currentObject != null) SelectObject();
            }
        }

        if (holdingObject == null) hand.GetComponent<Joint>().connectedBody = null;

        CheckForGround();

        if (Input.GetKey(KeyCode.Space)) Jump();

        rb.drag = Drag;      
    }

    //Move the player
    private void Move()
    {
        //Check where the player wants to go
        int forward = 0, right = 0;
        if (Input.GetKey(KeyCode.W)) forward++;
        if (Input.GetKey(KeyCode.S)) forward--;
        if (Input.GetKey(KeyCode.D)) right++;
        if (Input.GetKey(KeyCode.A)) right--;

        //Apply a force to move player in desired direction
        Vector3 movementDir = transform.forward * forward + transform.right * right;
        rb.AddForce(movementDir.normalized * MoveSpeed * Acceleration, ForceMode.Force);
    }

    //prevent the player's speed from being > MoveSpeed
    private void ClampSpeed()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (vel.magnitude > MoveSpeed)
        {
            Vector3 limVel = vel.normalized * MoveSpeed;
            rb.velocity = new Vector3(limVel.x, rb.velocity.y, limVel.z);
        }
    }

    //Changes where the player is looking based on the mouse position
    private void UpdateRotation()
    {
        yaw += horizontalRotateSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalRotateSpeed * Input.GetAxis("Mouse Y");

        cam.transform.localEulerAngles = new Vector3(Mathf.Clamp(pitch, pitchBounds.x, pitchBounds.y), 0f, 0f);
        transform.eulerAngles = new Vector3(0f, yaw, 0f);
    }

    //Selection ray
    void Raycast()
    {
        //Create a ray from the camera to where the mouse is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, selectionRange)) //did we hit something?
        {
            ISelectable select;
            //did we hit something that is Selectable?
            if (hit.transform.gameObject.TryGetComponent(out select))
            {
                //hover over that object
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
        currentObject = null;
    }

    void SelectObject()
    {
        if (currentObject == null) return;

        currentObject.IsSelected = !currentObject.IsSelected;
    }

    void UnSelectObject()
    {
        if (hand.GetComponent<Joint>().connectedBody == null) return;

        (holdingObject as MonoBehaviour).transform.parent = null; //unparent object from hand
        holdingObject.IsSelected = false;
        holdingObject.IsHovered = false;
        hand.GetComponent<Joint>().connectedBody = null; //Unlink object from hand

        //renable collisions between object and player
        MyFunctions.IgnoreAllCollisions(transform, (holdingObject as MonoBehaviour).transform, false);
        
        holdingObject = null;
    }


    public void Pickup(ISelectable obj)
    {
        holdingObject = obj;
        MonoBehaviour objM = obj as MonoBehaviour;
        objM.transform.parent = hand.transform; //set object's parent to hand
        StartCoroutine(MoveObjectToOrigin(objM.transform));

        //ignore collisions between object and player
        MyFunctions.IgnoreAllCollisions(transform, objM.transform);
    }

    IEnumerator MoveObjectToOrigin(Transform obj)
    {
        Rigidbody oRb = obj.gameObject.GetComponent<Rigidbody>();
        oRb.isKinematic = true; //obj is not affected by forces
        /*
         * while the object isnt close enough to hand
         * move the object a little closer evey frame
         */
        while (obj.localPosition.magnitude > 0.05f)
        {
            obj.localPosition = Vector3.MoveTowards(obj.localPosition, Vector3.zero, pickupSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //close enough now
        obj.localPosition = Vector3.zero; //set actual position to (0,0,0) instead of close enough
        oRb.isKinematic = false; //renable forces on obj
        hand.GetComponent<Joint>().connectedBody = oRb; //link obj to hand
    }

    void CheckForGround()
    {
        //gets all colliders within feetRadius of the feet that are considered to be ground
        Collider[] collisions = Physics.OverlapSphere(feet.position, feetRadius, groundLayer);
        
        //did we find any colliders at all?
        isGrounded = collisions.Length > 0;
    }

    void Jump()
    {
        //if we are in the air or the cooldown isn't ready, dont't jump
        if (!isGrounded || !readyToJump) return;

        //adds a vertical force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        readyToJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);

    }

    void ResetJump()
    {
        readyToJump = true;
    }
}
