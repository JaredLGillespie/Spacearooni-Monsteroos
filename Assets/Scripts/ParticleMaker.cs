using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : MonoBehaviour
{
    [SerializeField] private GameObject ParticleObject; // Object to create
    [SerializeField] private float ParticleSpread = 0.0f; // Spread of objects
    [SerializeField] private int ParticleSetCount = 1; // Number of object to create at a time
    [SerializeField] private float DelayBetweenCreations = 1.0f; // Delay between object set creations
    [SerializeField] private bool Run = true; // Whether to create the particles
    [SerializeField] private bool RandomizeRotation = true; // Whether to rotate randomly around z-axis

    private IEnumerator createParticleCoroutine;

    private void Start()
    {
        if (Run)
        {
            createParticleCoroutine = CreateParticles();
            StartCoroutine(createParticleCoroutine);
        }

    }

    private IEnumerator CreateParticles()
    {
        while (true)
        {
            for (var i = 0; i < ParticleSetCount; i++)
            {
                var position = this.transform.position + new Vector3(Random.Range(-ParticleSpread, ParticleSpread), Random.Range(-ParticleSpread, ParticleSpread), 0).normalized * ParticleSpread;
                var obj = Instantiate(ParticleObject, position, this.transform.rotation);

                if (RandomizeRotation)
                    obj.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            }
            yield return new WaitForSeconds(DelayBetweenCreations);
        }
    }

    public void StartParticles()
    {
        Run = true;

        if (createParticleCoroutine == null)
        {
            StartCoroutine(createParticleCoroutine);
        }
        else
        {
            createParticleCoroutine = CreateParticles();
            StartCoroutine(createParticleCoroutine);
        }
    }

    public void StopParticles()
    {
        Run = false;

        if (createParticleCoroutine != null)
        {
            StopCoroutine(createParticleCoroutine);
            createParticleCoroutine = null;
        }
    }
}
