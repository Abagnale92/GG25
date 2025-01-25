using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private BoxCollider2D spawnZone;
    public float timerSpawner;
    public float _timerSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnZone = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _timerSpawner -= Time.deltaTime;
        if(_timerSpawner <= 0)
        {

            float randomX = Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x);
            float randomY = Random.Range(spawnZone.bounds.min.y, spawnZone.bounds.max.y);

            Vector2 spawnPosition = new Vector2(randomX, randomY);
            //Vector2 coord = new Vector2(Random.Range(-spawnZone.bounds.extents.x , spawnZone.bounds.extents.x), Random.Range(-spawnZone.bounds.extents.y, spawnZone.bounds.extents.y));
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            _timerSpawner = timerSpawner;
        }
    }
}
