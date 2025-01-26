using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float maxSpeed = 15f;  
    [SerializeField] GameObject projectilePrefab;
    public GameObject powerUpTextPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.5f;
    private float nextFireTime = 0f;
    private bool isTripleShotActive = false;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    private float lastMoveDirection = 1f; // Default moving right

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Shoot();
        
        /*
        if(rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.AddForce(rb.linearVelocity * -1 * Time.deltaTime * 10 );
        }
        */

    }

    void Move() 
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput != 0)
        {
            lastMoveDirection = moveInput > 0 ? 1 : -1;
            rb.AddForce(Vector2.right * moveInput * speed * Time.deltaTime * 60 * (1 + GameManager.instance.slipperyLevel/5));
        }
        else
        {
            // Continue moving in the last direction without additional force
            rb.AddForce(Vector2.right * lastMoveDirection * speed * Time.deltaTime * 20 * (1 + GameManager.instance.slipperyLevel / 5));
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    public void GameOver()
    {
        animator.SetBool("Lost", true);
    }

    public void Restart()
    {
        animator.SetBool("Lost", false);
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            if (isTripleShotActive)
            {
                ShootTriple();
            }
            else
            {
                AudioManager.instance.Play("throw");
                Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootTriple()
    {
        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity); // Centrale
        Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, 15)); // Destra
        Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, -15)); // Sinistra
        AudioManager.instance.Play("throw");
        AudioManager.instance.Play("throw");
        AudioManager.instance.Play("throw");
    }

    // Metodo per attivare i Power-ups
    public void ActivatePowerUp(PowerUpType type, float duration)
    {
        GameObject pref = Instantiate(powerUpTextPrefab, transform.position, Quaternion.identity, GameManager.instance.transform);
        TextMeshProUGUI txt = pref.GetComponentInChildren<TextMeshProUGUI>();
        UIManager.instance.AddPowerUpStatus(type);
        switch (type)
        {
            case PowerUpType.FireRate:
                txt.text = "SHOOT FASTER";
                StartCoroutine(FireRateBoost(duration));
                break;
            case PowerUpType.Speed:
                txt.text = "LESS SLIPPERY";
                StartCoroutine(SpeedBoost(duration));
                break;
            case PowerUpType.TripleShot:
                txt.text = "MORE EGGS!";
                StartCoroutine(TripleShotBoost(duration));
                break;
        }
    }

    IEnumerator FireRateBoost(float duration)
    {
        fireRate /= 2; // Dimezza il tempo tra i colpi (spara più velocemente)
        yield return new WaitForSeconds(duration);
        fireRate = 0.5f; // Ripristina la velocità di fuoco originale
    }

    IEnumerator SpeedBoost(float duration)
    {
        GameManager.instance.slipperyLevel -= 2;
        yield return new WaitForSeconds(duration);
     
    }

    IEnumerator TripleShotBoost(float duration)
    {
        isTripleShotActive = true;
        yield return new WaitForSeconds(duration);
        isTripleShotActive = false;
    }
}
