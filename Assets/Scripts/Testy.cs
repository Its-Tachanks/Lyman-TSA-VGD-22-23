using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string Test()
        {
            return "Test";
        }

        Debug.Log(Test());
    }
}