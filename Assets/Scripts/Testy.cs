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

    /*
    public static T[] GetComponentsInHierarchy<T>(this GameObject go)
    {
        List<T> components = new List<T>();
        components.AddRange(go.GetComponents<T>());
        foreach (Transform child in go.transform)
        {
            components.AddRange(child.gameObject.GetComponentsInHierarch<T>());
        }
        return components.ToArray();
    }
    */
}