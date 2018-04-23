using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CyclicMovement : MonoBehaviour
{
    [SerializeField] private float CycleInterval = 1f; // The number of seconds a full cycle is performed
    [SerializeField] private float Amplitude = 1f; // Amplitude to apply to cycle

    private new Rigidbody2D rigidbody2D;
    private bool isDirectionUp; // I ain't makin' an ENUM and a string is just silly

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        isDirectionUp = System.Convert.ToBoolean(Random.Range(0, 2)); // Randomize start direction
        StartCoroutine("FlipDirection");
    }

    private void FixedUpdate()
    {
        // This is more or less a zig-zag but oh well...
        if (isDirectionUp)
            rigidbody2D.position += (Vector2)this.gameObject.transform.up * Amplitude * Time.deltaTime;
        else
            rigidbody2D.position -= (Vector2)this.gameObject.transform.up * Amplitude * Time.deltaTime;
    }

    private IEnumerator FlipDirection()
    {
        // Do quarter cycle first so center is correct
        yield return new WaitForSeconds(CycleInterval / 4);
        isDirectionUp = !isDirectionUp;

        // Cycle directions
        while (true)
        {
            yield return new WaitForSeconds(CycleInterval / 2);
            isDirectionUp = !isDirectionUp;
        }
    }
}
