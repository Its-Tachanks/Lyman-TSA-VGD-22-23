using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCartSceneManager : MonoBehaviour
{
    public static RaceCartSceneManager instance { get; private set; }

    [Header("Boost Info")]
    [SerializeField] private Animator boostInfoAnim;
    [SerializeField] private Transform boostInfoStartTrigger, boostInfoEndTrigger;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple " + GetType() + "s in the scene");
        }
    }

    private void Update()
    {
        boostInfoAnim.SetBool("visible", Player.instance.transform.position.x > boostInfoStartTrigger.position.x && Player.instance.transform.position.x < boostInfoEndTrigger.position.x);
    }
}
