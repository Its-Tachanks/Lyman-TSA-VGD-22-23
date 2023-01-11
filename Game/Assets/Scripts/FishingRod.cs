using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour, IStorable
{
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

    [SerializeField] private QuickOutline outline;

    [SerializeField] private Vector3 holdingLocalPosition;
    [SerializeField] private Vector3 holdingLocalRotation;

    [SerializeField] private GameObject bobberPrefab;
    private GameObject currentBobber;

    [SerializeField] private Transform castPoint;
    [SerializeField] private float castForce;

    // Start is called before the first frame update
    void Start()
    {
        if (outline == null) outline = GetComponent<QuickOutline>();

        outline.enabled = false;
        DefaultLayer = gameObject.layer;
    }

    public void OnClick()
    {
        if (currentBobber == null)
        {
            CastBobber();
        }
    }

    private void CastBobber()
    {
        currentBobber = Instantiate(bobberPrefab, castPoint.position, Quaternion.identity);

        currentBobber.GetComponent<Rigidbody>().AddForce(Player.instance.transform.forward * castForce, ForceMode.Impulse);
    }

    [ContextMenu("Set Pos")]
    public void SetHoldingTransform()
    { 
        transform.localPosition = holdingLocalPosition;
        transform.localEulerAngles = holdingLocalRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
