using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyFunctions;

public class RideCart : MonoBehaviour, ISelectable
{
    [Header("Points")]
    [SerializeField] private Transform lockingPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Keys")]
    [SerializeField] private KeyCode exitKey;
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode brakeKey = KeyCode.S;

    [Header("Zoom Zoom bitch")]
    [SerializeField] private float zoom;
    [SerializeField] private float fallZoom;
    [SerializeField] private Rigidbody rb;


    [Header("Grounding")]
    [SerializeField] private Transform feet;
    [SerializeField, Range(0.01f, 1)] private float feetRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private bool isPlayerIn;

    private bool isHovered;
    public bool IsHovered //Implementation of IStorable
    {
        get
        {
            return isHovered;
        }
        set
        {
            isHovered = value;

            if (isHovered && !isSelected) //OnHover
            {

            }
            else //OnUnHover
            {

            }
        }
    }

    private bool isSelected;
    public bool IsSelected //Implementation of IStorable
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;

            if (isSelected) //OnSelect
            {
                LockPlayer();
            }
            else //OnDeselect
            {

            }
        }
    }


    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isPlayerIn) PlayerInActions();

        CheckForGround();

        transform.forward = new Vector3(transform.forward.x, Mathf.Clamp(transform.forward.y, -0.75f, 1f), transform.forward.z);
    }

    private void CheckForGround()
    {
        //gets all colliders within feetRadius of the feet that are considered to be ground
        Collider[] collisions = Physics.OverlapSphere(feet.position, feetRadius, groundLayer);

        //did we find any colliders at all?
        isGrounded = collisions.Length > 0;
    }

    private void PlayerInActions()
    {
        if (!isPlayerIn) return;

        if (Input.GetKeyDown(exitKey))
        {
            UnlockPlayer();
        }

        if (Input.GetKey(forwardKey))
        {
            rb.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * zoom);
        }
        else if (Input.GetKey(brakeKey))
        {
            rb.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * -zoom);
        }
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x = 0;
        rb.velocity = transform.TransformDirection(localVelocity);

        rb.AddForce(Vector3.down * fallZoom);

        if (Input.GetKeyDown(KeyCode.Space)) Boost();
    }

    private void LockPlayer()
    {
        if (isPlayerIn) return;

        Player.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        IgnoreAllCollisions(Player.instance.transform, transform);

        Player.instance.transform.parent = lockingPoint;
        Player.instance.transform.localPosition = Vector3.zero;

        isPlayerIn = true;       
    }

    private void UnlockPlayer()
    {
        if (!isPlayerIn) return;

        Player.instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        IgnoreAllCollisions(Player.instance.transform, transform, false);

        Player.instance.transform.parent = null; //orphan lol
        Player.instance.transform.position = exitPoint.position;

        isPlayerIn = false;
    }

    private void Boost()
    {
        Ray ray1 = new Ray(lockingPoint.position, Vector3.up);
        Ray ray2 = new Ray(lockingPoint.position, Vector3.down);
        RaycastHit hit1, hit2;
        if (Physics.Raycast(ray1, out hit1) && Physics.Raycast(ray2, out hit2))
        {
            BoostRing ring;
            if (hit1.transform.gameObject.Equals(hit2.transform.gameObject) && hit1.transform.TryGetComponent(out ring))
            {
                Debug.Log("Boost");
                rb.AddForce(new Vector3(transform.forward.x, 0f, transform.forward.z) * ring.GetBoost(), ForceMode.Impulse);
                return;
            }
        }
        Debug.Log("No boost");
    }

}
