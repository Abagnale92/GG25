using UnityEngine;
using UnityEngine.UI;

public class StatusTimer : MonoBehaviour
{

    ProgressBar bar;
    public Image powerUpIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar = GetComponent<ProgressBar>();
        bar.minimum = 0;
        bar.maximum = 10;
        bar.current = bar.maximum;
    }

    public void SetIcon(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.FireRate:
                powerUpIcon.sprite = UIManager.instance.powerUpSprites[0];
                break;
            case PowerUpType.Speed:
                powerUpIcon.sprite = UIManager.instance.powerUpSprites[1];
                break;
            case PowerUpType.TripleShot:
                powerUpIcon.sprite = UIManager.instance.powerUpSprites[2];
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bar.current -= Time.deltaTime;

        if(bar.current <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
