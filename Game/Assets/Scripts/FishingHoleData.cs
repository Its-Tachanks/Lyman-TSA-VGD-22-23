using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FishingHoleData", menuName = "My Scriptable Objects/Fishing Hole Data")]
public class FishingHoleData : ScriptableObject
{
    [SerializeField, Range(1f, 100f)] private float itemChance = 50f;
    [SerializeField] private ItemPair[] itemPool;

    public GameObject GetRandomObject()
    {
        float totalProbability = 0;
        foreach (ItemPair pair in itemPool)
        {
            totalProbability += pair.chance;
        }

        float random = UnityEngine.Random.Range(0f, totalProbability);

        float runningSum = 0;
        foreach (ItemPair pair in itemPool)
        {
            runningSum += pair.chance;
            if (runningSum > random)
            {
                return pair.item;
            }
        }

        return null;
    }

    public float GetChance()
    {
        return itemChance;
    }
}

[System.Serializable]
class ItemPair
{
    public GameObject item;
    public float chance;

    public ItemPair(GameObject item, float chance)
    {
        this.item = item;
        this.chance = chance;
    }
}
