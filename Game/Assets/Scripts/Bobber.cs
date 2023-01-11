using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private Transform attatchmentPoint;
    private Transform returnPoint;

    [SerializeField] private float returnSpeed;
    [SerializeField] private GameObject waterParticles;

    private WaterBody currentBody;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float biteForce;
    [SerializeField] private float splashRate;
    [SerializeField] private float biteTime;

    [SerializeField] private float biteCheckRate;
    private float currentBiteTime;

    private bool biting;

    private bool enteredWater;

    private GameObject currentItem;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public Vector3 GetAttatchmentPoint()
    {
        return attatchmentPoint.position;
    }

    public void ReturnToRod(Transform point)
    {
        GetComponent<Collider>().isTrigger = true;
        rb.isKinematic = true;
        returnPoint = point;
    }

    private void Splash()
    {
        Instantiate(waterParticles, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out currentBody) && !enteredWater)
        {
            enteredWater = true;
            Splash();
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<WaterBody>())
        {
            currentBody = null;
        }
    }

    public IEnumerator HasItem(float duration, GameObject item)
    {
        if (biting) yield break;

        float elapsed = 0f;
        biting = true;

        while (elapsed < duration)
        {
            Splash();

            if (returnPoint != null)
            {
                ReelInItem(item);
                biting = false;
                yield break;
            }

            elapsed += duration / splashRate;
            yield return new WaitForSeconds(duration / splashRate);
        }

        biting = false;
    }

    private void ReelInItem(GameObject item)
    {
        currentItem = Instantiate(item, transform);
        currentItem.transform.localPosition = Vector3.zero;

        if (currentItem.GetComponent<Rigidbody>()) currentItem.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Returning()
    {
        if (returnPoint == null) return;

        if (Vector3.Distance(transform.position, returnPoint.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnPoint.position, returnSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = returnPoint.position;
            returnPoint = null;

            if (currentItem != null)
            {
                if (currentItem.GetComponent<Rigidbody>()) currentItem.GetComponent<Rigidbody>().isKinematic = false;
                currentItem.transform.parent = null;
            }

            Destroy(gameObject);
        }
    }

    private void CheckForBite()
    {
        if (UnityEngine.Random.Range(0f, 100f) > currentBody.holeData.GetChance()) return;

        StartCoroutine(HasItem(biteTime, currentBody.holeData.GetRandomObject()));
    }

    private void Update()
    {
        if (biting) rb.AddForce(-transform.up * biteForce, ForceMode.Force);

        Returning();

        if (currentBody != null && currentBody.holeData != null && !biting && returnPoint == null)
        {
            currentBiteTime += Time.deltaTime;
            if (currentBiteTime > biteCheckRate)
            {
                currentBiteTime = 0f;
                CheckForBite();
            }
        }
    }
}
