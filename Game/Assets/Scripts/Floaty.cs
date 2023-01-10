using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty : MonoBehaviour
{
    [Header("Drag")]
    [SerializeField] private float underWaterDrag = 3f;
    [SerializeField] private float underWaterAngularDrag = 1f;
    private float defaultDrag;
    private float defaultAngularDrag;

    [Header("Other Shit")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float floatingPower = 15f;
    [SerializeField] private Transform[] floaters;

    private bool underwater;
    private int floatersUnderwater;

    [ReadOnly, SerializeField] private WaterBody currentBody;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        defaultDrag = rb.drag;
        defaultAngularDrag = rb.angularDrag;
    }

    private void FixedUpdate()
    {
        floatersUnderwater = 0;
        foreach (Transform floater in floaters)
        {
            float difference = GetDifference(floater);

            if (difference < 0)
            {
                rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floater.position, ForceMode.Force);
                floatersUnderwater++;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
            
        }

        if (underwater && floatersUnderwater == 0)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    private void SwitchState(bool isUnderwater)
    {
        rb.drag = isUnderwater ? underWaterDrag : defaultDrag;
        rb.angularDrag = isUnderwater ? underWaterAngularDrag : defaultAngularDrag;
    }

    private float GetDifference(Transform floater)
    {
        if (currentBody == null) return 0;

        return floater.position.y - currentBody.SurfaceHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.TryGetComponent(out currentBody);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<WaterBody>())
        {
            currentBody = null;
        }
    }
}
