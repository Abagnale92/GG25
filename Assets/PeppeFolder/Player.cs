using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.5f;
    private float nextFireTime = 0f;
    private bool isTripleShotActive = false;

    private void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * moveInput * speed * Time.deltaTime);
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
    }

    // Metodo per attivare i Power-ups
    public void ActivatePowerUp(PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUpType.FireRate:
                StartCoroutine(FireRateBoost(duration));
                break;
            case PowerUpType.Speed:
                StartCoroutine(SpeedBoost(duration));
                break;
            case PowerUpType.TripleShot:
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
        speed *= 2; // Doppia velocità di movimento
        yield return new WaitForSeconds(duration);
        speed = 5f; // Ripristina la velocità originale
    }

    IEnumerator TripleShotBoost(float duration)
    {
        isTripleShotActive = true;
        yield return new WaitForSeconds(duration);
        isTripleShotActive = false;
    }
}
