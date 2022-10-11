using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, ISelectable
{
    private bool isHovered;
    public bool IsHovered
    {
        get
        {
            return isHovered;
        }
        set
        {
            isHovered = value;

            if (isHovered && !isSelected)
            {
                rend.material.color = hoverColor;
            }
            else if (!isSelected)
            {
                rend.material.color = defaultColor;
            }
        }
    }

    private bool isSelected;
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;

            if (isSelected)
            {
                rend.material.color = selectedColor;
                LaunchUp();
            }
            else if (!isHovered)
            {
                rend.material.color = defaultColor;
            }
        }
    }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer rend;
    [SerializeField, Range(0, 20)] private float launchSpeed;

    private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rend == null) rend = GetComponent<MeshRenderer>();

        defaultColor = rend.material.color;
    }

    private void LaunchUp()
    {
        rb.AddForce(Vector3.up * launchSpeed, ForceMode.Impulse);
    }
}
