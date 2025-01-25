using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] string targetTag = "Enemy"; // Tag del bersaglio
    [SerializeField] float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(targetTag))
        {
            Debug.Log("Colpito!");
            GameManager.instance.IncreaseScore(10);
            Destroy(this.gameObject);
        }
    }
}

