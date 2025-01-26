using UnityEngine;

public enum PowerUpType { FireRate, Speed, TripleShot }

public class PowerUp : MonoBehaviour
{
    [SerializeField] public PowerUpType powerUpType;
    [SerializeField] float duration = 5f; // Tempo di durata del potenziamento


    public SpriteRenderer spriteRender;

    private void Start()
    {
        UpdateGraphics();

    }

    public void UpdateGraphics()
    {
        switch (powerUpType)
        {
            case PowerUpType.FireRate:
                spriteRender.sprite = UIManager.instance.powerUpSprites[0];
                break;
            case PowerUpType.Speed:
                spriteRender.sprite = UIManager.instance.powerUpSprites[1];
                break;
            case PowerUpType.TripleShot:
                spriteRender.sprite = UIManager.instance.powerUpSprites[2];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            AudioManager.instance.Play("powerUp");
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
