using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartRotateSwitch : Switch
{
    [SerializeField] private Rail[] assignedRails;

    [Header("Graphics")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Color onColor = Color.green, offColor = Color.red;

    private int finishedRails;
    private bool isRotating;

    private void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        rend.material.color = offColor;
    }

    public void NotifySwitch()
    {
        finishedRails++;
        if (finishedRails >= assignedRails.Length) isRotating = false;
    }

    protected override void OnSelect()
    {
        if (isRotating)
        {
            isSelected = !isSelected;
            return;
        }

        isRotating = true;
        finishedRails = 0;

        foreach (Rail r in assignedRails)
        {
            r.Rotate(90f, this);
        }
        rend.material.color = onColor;
    }

    protected override void OnDeselect()
    {
        if (isRotating)
        {
            isSelected = !isSelected;
            return;
        }

        isRotating = true;
        finishedRails = 0;

        foreach (Rail r in assignedRails)
        {
            r.Rotate(-90f, this);
        }
        rend.material.color = offColor;
    }
}
