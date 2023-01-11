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
    private Bobber currentBobber;

    [SerializeField] private Transform castPoint;
    [SerializeField] private float castForce;
    [SerializeField] private LineRenderer line;

    [SerializeField] private float maxBobberDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (outline == null) outline = GetComponent<QuickOutline>();
        if (line == null) line = GetComponent<LineRenderer>();

        outline.enabled = false;
        DefaultLayer = gameObject.layer;
    }

    private void OnDisable()
    {
        if (currentBobber != null) Destroy(currentBobber.gameObject);
        line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
    }

    public void OnClick()
    {
        if (currentBobber == null)
        {
            CastBobber();
        }
        else
        {
            currentBobber.ReturnToRod(castPoint);
        }
    }

    private void CastBobber()
    {
        currentBobber = Instantiate(bobberPrefab, castPoint.position, Quaternion.identity).GetComponent<Bobber>();

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
        if (currentBobber != null)
        {
            line.SetPositions(new Vector3[] { castPoint.position, currentBobber.GetAttatchmentPoint() });
        }
        else
        {
            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        }

        if (currentBobber != null && Vector3.Distance(castPoint.position, currentBobber.GetAttatchmentPoint()) > maxBobberDistance)
        {
            Destroy(currentBobber.gameObject);
        }
    }
}
