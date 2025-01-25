using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private BoxCollider2D spawnZone;
    public float timerSpawner;
    public float _timerSpawner;
    public float gameTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnZone = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _timerSpawner -= Time.deltaTime;
        gameTime += Time.deltaTime;
        if(_timerSpawner <= 0)
        {
            SpawnBubble();
            _timerSpawner = timerSpawner - Mathf.Clamp((Mathf.Log(gameTime) + 1), 1, 9f)/2;
            /*
                        float randomX = Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x);
                        float randomY = Random.Range(spawnZone.bounds.min.y, spawnZone.bounds.max.y);

                        Vector2 spawnPosition = new Vector2(randomX, randomY);
                        //Vector2 coord = new Vector2(Random.Range(-spawnZone.bounds.extents.x , spawnZone.bounds.extents.x), Random.Range(-spawnZone.bounds.extents.y, spawnZone.bounds.extents.y));
                        Instantiate(prefab, spawnPosition, Quaternion.identity);
                        _timerSpawner = timerSpawner; */
        }
    }

    void SpawnBubble()
    {
        float randomX = Random.Range(spawnZone.bounds.min.x, spawnZone.bounds.max.x);
        float randomY = Random.Range(spawnZone.bounds.min.y, spawnZone.bounds.max.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Instantiate the bubble
        GameObject newBubble = Instantiate(prefab, spawnPosition, Quaternion.identity);


        // Randomize the bubble size around the evaluated average (add variation)
        int bubbleSize = Mathf.Clamp((((Mathf.FloorToInt(Random.Range(1, (Mathf.Log(gameTime) + 1)))/2) *2)),1,16);

        // Set the bubble size in the BubbleLogic script
        BubbleLogic bubbleLogic = newBubble.GetComponent<BubbleLogic>();

        float rand = Random.Range(0, 100);
        if(rand <= 20)
        {
            rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    bubbleLogic.SetPowerUp(PowerUpType.FireRate);
                    break;
                case 1:
                    bubbleLogic.SetPowerUp(PowerUpType.Speed);
                    break;
                case 2:
                    bubbleLogic.SetPowerUp(PowerUpType.TripleShot);
                    break;
            }
            
        }

        if (bubbleLogic != null)
        {
            bubbleLogic.BubbleSize = bubbleSize;
        }
    }
}

