using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    [Header("Type")]
    public MineType type;
    [SerializeField] private Renderer[] typeAffectedParts;

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;

    [Header("Values")]
    [SerializeField] private float speedThreshold;

    [HideInInspector] public Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponent<Animator>();

        startingPos = rb.position;
    }

    public void ReturnToStart()
    {
        rb.position = startingPos;
    }

    public void AssignTypeMat(Material mat)
    {
        foreach (Renderer ren in typeAffectedParts)
        {
            ren.material = mat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isMoving", rb.velocity.magnitude > speedThreshold);
    }
}
