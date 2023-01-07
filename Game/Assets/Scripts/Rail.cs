using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, ISelectable
{
    [Header("Type")]
    public MineType type;
    [SerializeField] private Renderer[] typeAffectedParts;

    [Header("Rotation")]
    private Minecart currentCart;
    private bool isRotating;
    [SerializeField] private float rotateSpeed = 1f;

    [Header("Outline")]
    [SerializeField] public QuickOutline outline;

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
                OutlineSetActive(true);
            }
            else //OnUnHover
            {
                OutlineSetActive(false);
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
                Rotate(90f);
                isSelected = false;
            }
            else //OnDeselect
            {
                
            }
        }
    }

    private void Start()
    {
        if (outline == null) outline = GetComponent<QuickOutline>();
        outline.enabled = false;
    }

    public void AssignTypeMat(Material mat)
    {
        foreach (Renderer ren in typeAffectedParts)
        {
            ren.material = mat;
        }
        outline.OutlineColor = mat.color;
    }

    [ContextMenu("Rotate")]
    public void Rotate(float angle, CartRotateSwitch cartSwitch)
    {
        if (isRotating) return;

        isRotating = true;

        currentCart = null;
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.up, Color.magenta, 2f);
        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.transform.gameObject.TryGetComponent(out currentCart))
            {
                currentCart.transform.parent = transform;
            }
            else
            {
                Debug.Log("no cart");
            }
        }
        StartCoroutine(Spin(angle, cartSwitch));
    }

    public void Rotate(float angle)
    {
        Rotate(angle, null);
    }

    IEnumerator Spin(float angle, CartRotateSwitch cartSwitch)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up) * initialRotation;

        float t = 0f;
        while (t < 1f)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            t += Time.deltaTime * rotateSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.rotation = targetRotation;

        if (currentCart != null)
        {
            currentCart.transform.parent = null;
            currentCart = null;
        }

        isRotating = false;
        if (cartSwitch != null) cartSwitch.NotifySwitch();
    }

    public void OutlineSetActive(bool active)
    {
        outline.enabled = active;
    }
}
