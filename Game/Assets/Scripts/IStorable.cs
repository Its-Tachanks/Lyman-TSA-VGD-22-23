using UnityEngine;
using System;

public interface IStorable : ISelectable
{
    public Sprite InventoryIcon { get; set; }
    public LayerMask DefaultLayer { get; set; }
}
