using UnityEngine;

public class Ball : MonoBehaviour, IStorable
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
                rend.material.color = hoverColor;
            }
            else //OnUnHover
            {
                if (!isSelected)
                {
                    rend.material.color = defaultColor;
                }
                else
                {
                    rend.material.color = selectedColor;
                }
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
                rend.material.color = selectedColor;
            }
            else //OnDeselect
            {
                LaunchForward();
                if (!isHovered)
                {
                    rend.material.color = defaultColor;
                }
                else
                {
                    rend.material.color = hoverColor;
                }
            }
        }
    }

    [field: SerializeField] public Sprite InventoryIcon { get; set; }
    public LayerMask DefaultLayer { get; set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer rend;
    [SerializeField, Range(0, 20)] private float launchSpeed;
    [SerializeField] private float verticalBoost;

    private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor;

    //get references if not assgined
    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rend == null) rend = GetComponent<MeshRenderer>();

        defaultColor = rend.material.color;
        DefaultLayer = gameObject.layer;
    }


    //apply a vertical force
    private void LaunchUp()
    {
        rb.AddForce(Vector3.up * launchSpeed, ForceMode.Impulse);
    }

    //apply a slightly up forward force
    private void LaunchForward()
    {
        rb.AddForce((Camera.main.transform.forward + new Vector3(0f, verticalBoost, 0f)).normalized * launchSpeed, ForceMode.Impulse);
    }
}
