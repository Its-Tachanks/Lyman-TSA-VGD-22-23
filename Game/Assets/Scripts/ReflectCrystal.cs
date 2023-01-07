using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCrystal : MonoBehaviour, IStorable
{
    [Header("Line")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform origin;
    [SerializeField] private float maxDistance;

    [SerializeField, Tooltip("Extends the beam this distance into whatever object is hit")] 
    private float tipDistance;

    [Header("Reflection")]
    [SerializeField, Range(0f, 360f)] private float reflectAngle;
    [SerializeField] private float cooldown;
    private float cooldownTimer;

    [Header("Random Shit")]

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
                outline.enabled = true;
            }
            else //OnUnHover
            {
                outline.enabled = false;
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
                if (!Player.instance.Store(this))
                {
                    isSelected = false;
                    return;
                }
            }
            else //OnDeselect
            {
                //rb.isKinematic = true;
                //LaunchForward();
                if (!isHovered)
                {
                    outline.enabled = false;
                }
                else
                {
                    outline.enabled = true;
                }
            }
        }
    }

    [field: SerializeField] public Sprite InventoryIcon { get; set; }
    public LayerMask DefaultLayer { get; set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private QuickOutline outline;

    private void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (outline == null) outline = GetComponent<QuickOutline>();

        outline.enabled = false;
        DefaultLayer = gameObject.layer;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer > cooldown)
        {
            UpdateLaser(origin.position);
        }

        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
    
        if (transform.parent != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void ReceiveLaser(Vector3 inDir)
    {
        cooldownTimer = 0f;

        Vector3 dir = Quaternion.AngleAxis(reflectAngle, Vector3.up) * inDir;

        Ray ray = new Ray(origin.position + dir * 0.6f, dir);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            
            UpdateLaser(hit.point + dir * tipDistance);

            ReflectCrystal reflectCrystal;
            CrystalGoal crystalGoal;

            if (hit.transform.gameObject.TryGetComponent(out reflectCrystal))
            {
                reflectCrystal.ReceiveLaser(dir);
            }
            else if (hit.transform.gameObject.TryGetComponent(out crystalGoal))
            {
                crystalGoal.ReceiveLaser();
            }
        }
        else
        {
            UpdateLaser(origin.position + dir * maxDistance);
        }
        Debug.DrawRay(origin.position, dir * 3, Color.red, 0.1f);
    }

    private void UpdateLaser(Vector3 end)
    {
        line.SetPositions(new Vector3[] { origin.position, end });
    }
}
