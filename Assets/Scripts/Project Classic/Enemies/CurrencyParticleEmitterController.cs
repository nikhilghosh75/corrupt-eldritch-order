using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class CurrencyParticleEmitterController : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystemRenderer renderer;
    private EmissionModule emissionModule;
    private CollisionModule collisionModule;
    private Transform targetTransform;
    private CurrencyType currencyType;
    private int numParticles;
    private ParticleSystem.Particle[] particles;
    private bool returning = false;
    private float particleReturnSpeed = 15.0f;
    private float currencyAccelerationFactor = 0.5f;

    private void Update()
    {
        // Destroy the emitter when no more particles are being emitted
        if (returning && ps.particleCount == 0)
        {
            Destroy(gameObject);
        }

        // Only update positions if the particles are returning to the target
        if (!returning)
            return;

        for (int i = numParticles - 1; i >= 0; i--)
        {
            ParticleSystem.Particle p = particles[i];

            Vector3 particleWorldPosition = ps.transform.TransformPoint(p.position);
            Vector3 direction = targetTransform.position - particleWorldPosition;

            // Particle has reached the target, 
            if (direction.magnitude < 0.5f && p.remainingLifetime > 0.01f)
            {
                p.remainingLifetime = 0.000000000001f; // Set lifetime to a very small value to make it disappear
                p.size = 0;
                particles[i] = p; // Update the particle in the array
                if (currencyType == CurrencyType.Run)
                {
                    if (RunManager.Instance)
                    {
                        RunManager.Instance.AddCurrency(0, 1);
                    }
                }
                else if (currencyType == CurrencyType.Permanent)
                {
                    if (RunManager.Instance)
                    {
                        RunManager.Instance.AddCurrency(1, 0);
                    }
                }
                continue;
            }

            // Move particle towards the target
            currencyAccelerationFactor += Time.deltaTime * 1.3f;
            Vector3 newPos = particleWorldPosition + direction.normalized * particleReturnSpeed * currencyAccelerationFactor * Time.deltaTime;
            p.position = ps.transform.InverseTransformPoint(newPos);
            particles[i] = p; // Update the particle in the array
        }

        ps.SetParticles(particles, particles.Length);
    }

    public void EmitCurrency(Transform target, CurrencyType type, int amount, float emissionTime, Material sprite, float delayTime, float returnSpeed)
    {
        ps = GetComponent<ParticleSystem>();
        renderer = GetComponent<ParticleSystemRenderer>();
        emissionModule = ps.emission;
        collisionModule = ps.collision;
        

        targetTransform = target;
        currencyType = type;
        particleReturnSpeed = returnSpeed;

        if (ps == null)
        {
            Debug.Log("null ps");
            return;
        }

        Burst burst = new Burst(0, 1, amount, emissionTime / amount);
        emissionModule.SetBurst(0, burst);
        collisionModule.enabled = true; // just in case it was disabled
        renderer.material = sprite;
        returning = false;

        ps.Play();

        // Wait for the duration of the delay then initiate the return of the particles to the player
        StartCoroutine(DelayThenReturn(delayTime));
    }

    private IEnumerator DelayThenReturn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        // Get the number of active particles
        numParticles = ps.particleCount;

        // Disable collisions
        collisionModule.enabled = false;

        // Get a reference to all the particles
        particles = new ParticleSystem.Particle[numParticles];
        ps.GetParticles(particles);

        returning = true;
    }

}
