using UnityEngine;

public class DestroyOnParticleEnd : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // Get the ParticleSystem component attached to the same GameObject
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem == null)
        {
            Debug.LogError("No ParticleSystem component found on this GameObject.");
            Destroy(gameObject); // Destroy immediately if no particle system found
        }
    }

    void Update()
    {
        // Check if the particle system has stopped playing
        if (particleSystem != null && !particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
