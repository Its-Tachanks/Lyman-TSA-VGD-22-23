using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [Header("Type")]
    public MineType type;
    [SerializeField] private Renderer[] typeAffectedParts;

    [Header("Rotation")]
    private Minecart currentCart;
    private bool isRotating;
    [SerializeField] private float rotateSpeed = 1f;

    public void AssignTypeMat(Material mat)
    {
        foreach (Renderer ren in typeAffectedParts)
        {
            ren.material = mat;
        }
    }

    [ContextMenu("Rotate")]
    public void Rotate(float angle, CartRotateSwitch cartSwitch)
    {
        if (isRotating) return;

        isRotating = true;

        currentCart = null;
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.transform.gameObject.TryGetComponent(out currentCart))
            {
                currentCart.transform.parent = transform;
            }
        }
        StartCoroutine(Spin(angle, cartSwitch));
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
        cartSwitch.NotifySwitch();
    }
}
