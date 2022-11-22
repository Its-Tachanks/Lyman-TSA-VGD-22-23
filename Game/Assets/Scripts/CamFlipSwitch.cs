using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFlipSwitch : Switch
{
    [SerializeField] private Camera cam;
    [SerializeField] private KeyCode exitKey;

    private void Start()
    {
        cam.gameObject.SetActive(false);
    }

    protected override void OnSelect()
    {
        cam.gameObject.SetActive(true);
    }

    protected override void OnDeselect()
    {
        cam.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsSelected)
        {
            if (Input.GetKeyDown(exitKey))
            {
                IsSelected = false;
            }
        }
    }
}
