using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCart : MonoBehaviour
{
    [SerializeField] private Transform lockingPoint;
    [SerializeField] private float lockingRadius;

    private bool isChosenCart;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (transform.eulerAngles.y == 0f)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else if (transform.eulerAngles.y == 90f)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            Debug.LogError(name + " is not rotated properly");
        }

        isChosenCart = lockingPoint.childCount > 0;
    }

    private void Update()
    {
        if (isChosenCart) return;

        if (lockingPoint.childCount == 0)
        {
            Collider[] collisions = Physics.OverlapSphere(lockingPoint.position, lockingRadius);
            foreach (Collider c in collisions)
            {
                ReflectCrystal reflectCrystal;
                if (c.transform.root.TryGetComponent(out reflectCrystal))
                {
                    reflectCrystal.transform.parent = lockingPoint;
                    reflectCrystal.transform.localPosition = Vector3.zero;
                }
            }
        }
        else
        {
            lockingPoint.GetChild(0).localPosition = Vector3.zero;
        }
    }
}
