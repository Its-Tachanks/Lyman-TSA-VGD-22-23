using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour, ISelectable
{
    protected bool isHovered;
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
                OnHover();
            }
            else //OnUnHover
            {
                OnUnHover();
            }
        }
    }

    protected bool isSelected;
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
                OnSelect();
            }
            else //OnDeselect
            {
                OnDeselect();
            }
        }
    }

    protected virtual void OnHover() { }
    protected virtual void OnUnHover() { }
    protected virtual void OnSelect() { }
    protected virtual void OnDeselect() { }
}
