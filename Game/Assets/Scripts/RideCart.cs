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
    [SerializeField] private KeyCode backKey = KeyCode.S;

    [Header("Zoom Zoom bitch")]
    [SerializeField] private float zoom;
    [SerializeField, Range(0, 1)] private float airNoZoom;
    [SerializeField] private Rigidbody rb;

    [Header("Grounding")]
    [SerializeField] private Transform feet;
    [SerializeField, Range(0.01f, 1)] private float feetRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private bool isPlayerIn;
    private Vector3 targetUp;

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

    private float Zoom
    {
        get
        {
            if (isGrounded)
            {
                return zoom;
            }
            else
            {
                return zoom * airNoZoom; 
            }
        }
    }


    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isPlayerIn) PlayerInActions();

        CheckForGround();

        transform.right = new Vector3(transform.right.x, Mathf.Clamp(transform.right.y, -0.75f, 1f), transform.right.z);
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


        int dir = 0;
        if (Input.GetKey(forwardKey)) dir++;
        if (Input.GetKey(backKey)) dir--;
        rb.AddForce(new Vector3(transform.right.x,(isGrounded ? transform.right.y : 0f) , transform.right.z) * dir * Zoom, ForceMode.Force);

        if ((new Vector3 (rb.velocity.x, 0f, rb.velocity.z).normalized - transform.right).magnitude > 1f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

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
                rb.AddForce(new Vector3(transform.right.x, 0f, transform.right.z) * ring.GetBoost(), ForceMode.Impulse);
                return;
            }
        }
        Debug.Log("No boost");
    }

}
