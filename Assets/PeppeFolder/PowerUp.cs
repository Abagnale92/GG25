using UnityEngine;

public enum PowerUpType { FireRate, Speed, TripleShot }

public class PowerUp : MonoBehaviour
{
    [SerializeField] public PowerUpType powerUpType;
    [SerializeField] float duration = 5f; // Tempo di durata del potenziamento

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ActivatePowerUp(powerUpType, duration);
                Destroy(gameObject); // Distrugge il power-up dopo essere stato raccolto
            }
        }

        if (other.CompareTag("Ground"))
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

 
}
