using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public float SurfaceHeight { get; private set; }

    private Collider collider;

    public FishingHoleData holeData;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        SurfaceHeight = collider.bounds.max.y;
    }
}
