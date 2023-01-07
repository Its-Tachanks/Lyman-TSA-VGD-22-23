using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceCrystal : MonoBehaviour
{
    [Header("Line")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform origin;
    [SerializeField] private float maxDistance;
    [SerializeField] private float tipDistance;

    private void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Ray ray = new Ray(origin.position, transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            UpdateLaser(hit.point + transform.up * tipDistance);

            ReflectCrystal reflectCrystal;
            CrystalGoal crystalGoal;

            if (hit.transform.gameObject.TryGetComponent(out reflectCrystal))
            {
                reflectCrystal.ReceiveLaser(transform.up);
            }
            else if (hit.transform.gameObject.TryGetComponent(out crystalGoal))
            {
                crystalGoal.ReceiveLaser();
            }
        }
        else
        {
            UpdateLaser(origin.position + transform.up * maxDistance);
        }
    }

    private void UpdateLaser(Vector3 end)
    {
        line.SetPositions(new Vector3[] { origin.position, end });
    }
}
