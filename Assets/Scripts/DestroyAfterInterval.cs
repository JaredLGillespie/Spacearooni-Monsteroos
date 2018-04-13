using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterInterval : MonoBehaviour
{
    [SerializeField] private float DestroyInterval = 10.0f; // Destroy after this amount of time has passed

    private void Start()
    {
        StartCoroutine("DestroyMe");
    }

    private IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(DestroyInterval);

        Destroy(this.gameObject);
    }
}
