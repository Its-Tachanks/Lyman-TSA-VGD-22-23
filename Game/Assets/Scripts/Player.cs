using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform feet;

    [Header("Movement")]
    [SerializeField, Range(1, 10)] private float horizontalRotateSpeed;
    [SerializeField, Range(1, 10)] private float verticalRotateSpeed;
    [SerializeField] private Vector2 pitchBounds;
    [SerializeField, Range(1, 20)] private float moveSpeed;
    [SerializeField, Range(1, 20)] private float airSpeed;
    [SerializeField] private float shiftMultiplier;
    [SerializeField, Range(1, 100)] private float acceleration;
    [SerializeField, Range(1, 100)] private float airAcceleration;
    [SerializeField, Range(1, 20)] private float drag;
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
    [SerializeField] public GameObject hand;
    [SerializeField, Range(0, 100)] private float selectionRange;
    [SerializeField, Range(1, 100)] private float pickupSpeed;
    private ISelectable currentObject;
    private bool isHovering;
    private ISelectable holdingObject;
    [SerializeField] private LayerMask holdingLayer;
    [SerializeField] private LayerMask ignoreLayers;

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
                if (!isHovering) UnSelectObject();
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

        pitch = Mathf.Clamp(pitch, pitchBounds.x, pitchBounds.y);

        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, yaw, 0f);
    }

    //Selection ray
    void Raycast()
    {
        //Create a ray from the camera to where the mouse is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, selectionRange, ~ignoreLayers)) //did we hit something?
        {
            ISelectable select;
            //did we hit something that is Selectable?
            if (hit.transform.gameObject.TryGetComponent(out select))
            {
                if (currentObject != select) UnHover();

                //hover over that object
                currentObject = select;
                currentObject.IsHovered = true;

                isHovering = true;
            }
            else
            {
                UnHover();
                isHovering = false;
            }
        }
        else
        {
            UnHover();
            isHovering = false;
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
        if (holdingObject is FishingRod)
        {
            (holdingObject as FishingRod).OnClick();
            return;
        }

        if (holdingObject == null && hand.GetComponent<Joint>().connectedBody == null) return;

        Rigidbody oRb;
        if ((holdingObject as MonoBehaviour).TryGetComponent(out oRb))
        {
            oRb.isKinematic = false;
        }

        (holdingObject as MonoBehaviour).transform.parent = null; //unparent object from hand
        holdingObject.IsSelected = false;
        holdingObject.IsHovered = false;

        if (holdingObject is IStorable) (holdingObject as MonoBehaviour).gameObject.layer = (holdingObject as IStorable).DefaultLayer;

        hand.GetComponent<Joint>().connectedBody = null; //Unlink object from hand      

        //renable collisions between object and player
        MyFunctions.IgnoreAllCollisions(transform, (holdingObject as MonoBehaviour).transform, false);

        holdingObject = null;

        Inventory.instance.DropObject();
    }

    public bool Store(IStorable obj)
    {
        bool success = Inventory.instance.Store(obj);

        if (!success)
        {
            return false;
        }

        return true;
    }

    public void PocketObject()
    {
        if (hand.GetComponent<Joint>().connectedBody == null) return;

        (holdingObject as MonoBehaviour).transform.parent = null; //unparent object from hand
        hand.GetComponent<Joint>().connectedBody = null; //Unlink object from hand

        //renable collisions between object and player
        MyFunctions.IgnoreAllCollisions(transform, (holdingObject as MonoBehaviour).transform, false);

        (holdingObject as MonoBehaviour).gameObject.SetActive(false);
        (holdingObject as MonoBehaviour).transform.position += Vector3.up * 100;

        holdingObject = null;
    }

    public void Pickup(ISelectable obj)
    {
        holdingObject = obj;
        if (!obj.IsSelected) obj.IsSelected = true;
        MonoBehaviour objM = obj as MonoBehaviour;
        objM.transform.parent = hand.transform; //set object's parent to hand
        StartCoroutine(MoveObjectToOrigin(objM.transform));
        objM.gameObject.layer = (int)Mathf.Log(holdingLayer, 2);

        //ignore collisions between object and player
        MyFunctions.IgnoreAllCollisions(transform, objM.transform);
    }

    IEnumerator MoveObjectToOrigin(Transform obj)
    {
        Rigidbody oRb = null;
        if (obj.gameObject.GetComponent<Rigidbody>() != null)
        {
            oRb = obj.gameObject.GetComponent<Rigidbody>();
            oRb.isKinematic = true; //obj is not affected by forces
        }
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
                                          //if (oRb != null) oRb.isKinematic = false; //renable forces on obj
        if (oRb != null) hand.GetComponent<Joint>().connectedBody = oRb; //link obj to hand
        
        if (obj.GetComponent<FishingRod>())
        {
            obj.GetComponent<FishingRod>().SetHoldingTransform();
        }
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
