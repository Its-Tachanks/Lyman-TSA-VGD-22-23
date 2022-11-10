using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
  [ReadOnly] public int id;

  private bool isSelected;
  public bool IsSelected {
    get { return isSelected; }

    set {
      isSelected = value;
      border.effectColor = isSelected ? selectedBorderColor : defaultColor;
    }
  }

  [SerializeField] private Color selectedBorderColor = Color.black;
  private Color defaultColor;

  [SerializeField] private Outline border;
  [SerializeField] private Image icon;
  [ReadOnly] public IStorable storedObject;

  private void Awake() {
    if (border == null) border = GetComponent<Outline>();
    if (icon == null) icon = GetComponentInChildren<Image>();

    defaultColor = border.effectColor;
  }

  public void StoreObject(IStorable obj) {
    storedObject = obj;
    icon.sprite = obj.InventoryIcon;
  }

  public void RemoveObject() {
    storedObject = null;
    icon.sprite = null;
  }
}
