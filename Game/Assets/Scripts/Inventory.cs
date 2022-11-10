using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
  [SerializeField, Range(0, 9)] private int numberOfSlots;
  [SerializeField, ReadOnly] private InventorySlot[] slots;
  [SerializeField] private GameObject slotPrefab;
  [SerializeField] private GameObject slotParent;
  private int currentSlot = 0;

  public static Inventory instance { get; private set; }

  private void Awake() {
    if (instance == null) {
      instance = this;
    }
    else {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
  }

  private void Start() {
    foreach (Transform child in slotParent.transform) {
      Destroy(child.gameObject);
    }

    slots = new InventorySlot[numberOfSlots];
    for (int i = 0; i < numberOfSlots; i++) {
      GameObject s = Instantiate(slotPrefab, slotParent.transform);
      slots[i] = s.GetComponent<InventorySlot>();
      slots[i].id = i;
    }

    slots[currentSlot].IsSelected = true;
  }

  private void Update() {
    CheckForInput();
  }

  private void CheckForInput() {
    if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
    if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
    if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
    if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
    if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
    if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
    if (Input.GetKeyDown(KeyCode.Alpha8)) SelectSlot(7);
    if (Input.GetKeyDown(KeyCode.Alpha9)) SelectSlot(8);
  }

  private void SelectSlot(int index) {
    if (index >= numberOfSlots || index == currentSlot) return;

    if (slots[currentSlot].storedObject != null) {
      Player.instance.PocketObject();
    }

    slots[currentSlot].IsSelected = false;
    slots[index].IsSelected = true;
    currentSlot = index;

    if (slots[currentSlot].storedObject != null) {
      TakeOutObject();
    }
  }

  public bool Store(IStorable obj) {
    for (int i = 0; i < numberOfSlots; i++) {
      if (slots[i].storedObject != null) continue;

      slots[i].StoreObject(obj);

      if (currentSlot == i) {
        Player.instance.Pickup(obj);
      }
      else {
        (obj as MonoBehaviour).gameObject.SetActive(false);
      }

      return true;
    }

    return false;
  }

  public void TakeOutObject() {
    IStorable cur = slots[currentSlot].storedObject;
    (cur as MonoBehaviour).gameObject.SetActive(true);
    (cur as MonoBehaviour).transform.position = Player.instance.hand.transform.position + Vector3.down;
    Player.instance.Pickup(cur);
  }

  public void DropObject() {
    slots[currentSlot].RemoveObject();
  }
}
