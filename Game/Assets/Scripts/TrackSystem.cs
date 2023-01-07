using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSystem : MonoBehaviour
{
    [SerializeField] private Transform[] ends;

    private void Start()
    {
        foreach (Transform end in ends)
        {
            MyFunctions.IgnoreAllCollisions(end, Player.instance.transform);
        }
    }
}
