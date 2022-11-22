using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinecartPuzzleManager : MonoBehaviour
{
    public static MinecartPuzzleManager instance { get; private set; }
    [SerializeField] private Material[] typeMats = new Material[Enum.GetValues(typeof(MineType)).Length - 1];
    [SerializeField] private CartRotateSwitch[] switches = new CartRotateSwitch[Enum.GetValues(typeof(MineType)).Length - 1];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple " + GetType() + "s in this scene");
        }
    }

    private void Start()
    {
        AssignRails();
        AssignCarts();
        AssignCheckers();
    }

    private void AssignCarts()
    {
        foreach (Minecart c in FindObjectsOfType<Minecart>())
        {
            if (c.type == MineType.None) continue;

            c.AssignTypeMat(typeMats[(int)c.type - 1]);
        }
    }

    private void AssignCheckers()
    {
        foreach (CartChecker c in FindObjectsOfType<CartChecker>())
        {
            if (c.type == MineType.None) continue;

            c.AssignTypeMat(typeMats[(int)c.type - 1]);
        }
    }

    private void AssignRails()
    {
        foreach (Rail r in FindObjectsOfType<Rail>())
        {
            if (r.type == MineType.None) continue;

            r.AssignTypeMat(typeMats[(int)r.type - 1]);

            if (switches[(int)r.type - 1] != null)
            {
                switches[(int)r.type - 1].AddRail(r);
            }
        }
    }
}
