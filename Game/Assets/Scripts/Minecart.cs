using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour, ISelectable
{
    [Header("Type")]
    public MineType type;
    [SerializeField] private Renderer[] typeAffectedParts;

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;

    [Header("Values")]
    [SerializeField] private float speedThreshold;

    [HideInInspector] public Vector3 startingPos;

    [Header("Outline")]
    [SerializeField] public QuickOutline outline;

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
                OutlineSetActive(true);
            }
            else //OnUnHover
            {
                if (!isSelected) OutlineSetActive(false);
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
                OutlineSetActive(true);
            }
            else //OnDeselect
            {

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponent<Animator>();
        if (outline == null) outline = GetComponent<QuickOutline>();
        
        outline.enabled = false;

        startingPos = rb.position;
    }

    public void ReturnToStart()
    {
        rb.position = startingPos;
    }

    public void AssignTypeMat(Material mat)
    {
        foreach (Renderer ren in typeAffectedParts)
        {
            ren.material = mat;
        }
        outline.OutlineColor = mat.color;
    }

    public void OutlineSetActive(bool active)
    {
        outline.enabled = active;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isMoving", rb.velocity.magnitude > speedThreshold);
    }
}
