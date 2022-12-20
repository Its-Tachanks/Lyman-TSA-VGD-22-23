using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRing : MonoBehaviour
{
    [SerializeField] private float boosting;
    private bool boosted;

    public float GetBoost()
    {
        if (boosted) return 0f;

        boosted = true;

        return boosting;
    }
}
