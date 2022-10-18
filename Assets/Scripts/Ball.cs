using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, ISelectable
{
    private bool isHovered;
    public bool IsHovered //Implementation of ISelectable
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
            }
        }
    }

    private bool isSelected;
    public bool IsSelected //Implementation of ISelectable
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
                rend.material.color = selectedColor;
                Player.instance.Pickup(this);
            }
            else //OnDeselect
            {
                LaunchForward();
                if (!isHovered)
                {
                    rend.material.color = defaultColor;
                }
            }
        }
    }

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
