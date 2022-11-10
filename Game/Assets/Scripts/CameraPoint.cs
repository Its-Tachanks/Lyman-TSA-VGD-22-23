using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Old code to control the camera when there was no player
 */
public class CameraPoint : MonoBehaviour {

  [SerializeField] private float horizontalRotateSpeed;
  [SerializeField] private float verticalRotateSpeed;

  [SerializeField] private Rigidbody rb;
  [SerializeField, Range(1, 20)] private float moveSpeed;
  [SerializeField] private float shiftScale;

  private float yaw = 0f, pitch = 0f;

  private ISelectable currentObject;

  void Start() {
    if (rb == null) rb = GetComponent<Rigidbody>();
  }

  private void Update() {
    rb.velocity = Vector3.zero;

    if (Input.GetMouseButton(0)) UpdateRotation();

    if (Input.GetKey(KeyCode.W)) MoveForward();

    if (Input.GetKey(KeyCode.S)) MoveBackward();

    if (Input.GetKey(KeyCode.D)) MoveRight();

    if (Input.GetKey(KeyCode.A)) MoveLeft();

    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) IncreaseSpeed();

    Raycast();

    if (Input.GetMouseButton(1)) SelectObject();
  }

  void UpdateRotation() {
    yaw += horizontalRotateSpeed * Input.GetAxis("Mouse X");
    pitch -= verticalRotateSpeed * Input.GetAxis("Mouse Y");

    transform.eulerAngles = new Vector3(pitch, yaw, 0f);
  }

  void MoveForward() {
    rb.velocity += transform.forward.normalized * moveSpeed;
  }

  void MoveBackward() {
    rb.velocity += -transform.forward.normalized * moveSpeed;
  }

  void MoveRight() {
    rb.velocity += transform.right.normalized * moveSpeed;
  }

  void MoveLeft() {
    rb.velocity += -transform.right.normalized * moveSpeed;
  }

  void IncreaseSpeed() {
    rb.velocity *= shiftScale;
  }

  void Raycast() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 100)) {
      ISelectable select;
      if (hit.transform.gameObject.TryGetComponent<ISelectable>(out select)) {
        currentObject = select;
        currentObject.IsHovered = true;
      }
      else {
        UnHover();
      }
    }
    else {
      UnHover();
    }
  }

  void UnHover() {
    if (currentObject == null) return;

    currentObject.IsHovered = false;
    currentObject.IsSelected = false;
    currentObject = null;
  }

  void SelectObject() {
    if (currentObject == null) return;

    currentObject.IsSelected = !currentObject.IsSelected;
  }
}
