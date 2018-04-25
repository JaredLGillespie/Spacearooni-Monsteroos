using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCreator : MonoBehaviour
{
    [SerializeField] private GameObject ImpactObject; // Object to create on impact
    [SerializeField] private int NumberOfImpactObjects = 1; // Number of impact objects to create
    [SerializeField] private Vector2 ImpactObjectSpread = Vector2.zero; // Spread of impact objects
    [SerializeField] private float DelayBetweenImpact = 0.0f; // Delay between impact object creations
    [SerializeField] private bool StartImmediately = true; // Whether to start immediately
    [SerializeField] private bool DestroyWhenComplete = true; // Whether to destroy this when done

    private void Start()
    {
        if (StartImmediately)
           StartCoroutine("CreateImpact");
    }

    public IEnumerator CreateImpact()
    {
        return CreateImpactAtPosition(this.transform.position);
    }

    public IEnumerator CreateImpactAtPosition(Vector3 position)
    {
        var numRemainingImpacts = NumberOfImpactObjects;

        if (numRemainingImpacts > 0)
        {
            var pos = position + new Vector3(Random.Range(-ImpactObjectSpread.x, ImpactObjectSpread.x), Random.Range(-ImpactObjectSpread.y, ImpactObjectSpread.y), 0);
            var impact = Instantiate(ImpactObject, pos, this.transform.rotation);
            numRemainingImpacts--;
        }

        if (numRemainingImpacts > 0)
            yield return new WaitForSeconds(DelayBetweenImpact);

        while (numRemainingImpacts > 0)
        {
            var pos = position + new Vector3(Random.Range(-ImpactObjectSpread.x, ImpactObjectSpread.x), Random.Range(-ImpactObjectSpread.y, ImpactObjectSpread.y), 0);
            var impact = Instantiate(ImpactObject, pos, this.transform.rotation);
            numRemainingImpacts--;

            yield return new WaitForSeconds(DelayBetweenImpact);
        }

        if (DestroyWhenComplete)
            Destroy(this.gameObject);
    }
}
