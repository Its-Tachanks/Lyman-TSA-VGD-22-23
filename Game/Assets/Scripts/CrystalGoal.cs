using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGoal : MonoBehaviour
{
    [ReadOnly] public bool receivedLaser;

    public void ReceiveLaser()
    {
        if (receivedLaser) return;

        receivedLaser = true;
        Debug.Log("Crystal Puzzle Complete");
    }
}
