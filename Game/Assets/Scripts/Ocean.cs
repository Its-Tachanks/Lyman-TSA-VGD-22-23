using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    public static float oceanHeight;
    [SerializeField] private float surfaceOffset;

    private void Update()
    {
        oceanHeight = transform.position.y + surfaceOffset;
    }
}
