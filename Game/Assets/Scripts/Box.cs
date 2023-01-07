using UnityEngine;

public class Box : MonoBehaviour, IStorable
{
    [field: SerializeField] public Sprite InventoryIcon { get; set; }
    public LayerMask DefaultLayer { get; set; }
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
                if (!Player.instance.Store(this))
                {
                    isSelected = false;
                    return;
                }
            }
            else //OnDeselect
            {

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DefaultLayer = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
