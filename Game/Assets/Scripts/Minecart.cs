using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;

    [Header("Values")]
    [SerializeField] private float speedThreshold;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isMoving", rb.velocity.magnitude > speedThreshold);
    }
}
