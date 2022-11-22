using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartChecker : MonoBehaviour
{
    public MineType type;
    [SerializeField] private Renderer[] typeAffectedParts;
    [field: SerializeField]public bool IsComplete { get; private set; }
    [SerializeField] private ParticleSystem completeEffect;
    [SerializeField] private GameObject checkMark;

    private void Start()
    {
        checkMark.SetActive(false);
    }

    public void AssignTypeMat(Material mat)
    {
        foreach (Renderer ren in typeAffectedParts)
        {
            ren.material = mat;

            var main = completeEffect.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.white, mat.color);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Minecart cart = MyFunctions.GetComponentInHierarchy<Minecart>(other.transform);

        if (cart == null) return;

        if (cart.type == type)
        {
            IsComplete = true;
            completeEffect.Play();
            checkMark.SetActive(true);
        }
        else
        {
            cart.ReturnToStart();
        }
    }
}
