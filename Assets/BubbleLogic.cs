using UnityEngine;

public class BubbleLogic : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _swayTimer;
    public Transform powerUp;
    private PowerUp puComponent;
    private bool isPowerUp = false;

    public int BubbleSize = 1;
    public float splitForce = 5;
    public float upSwayForce;
    public float sideSwayForce;
    public AnimationCurve upSway;
    public float swayTimer;
    public float maxSideSpeed;


    private void Awake()
    {
        this._rb = GetComponent<Rigidbody2D>();
        _swayTimer = swayTimer;
        puComponent = powerUp.GetComponent<PowerUp>();
    }

    public void SetPowerUp(PowerUpType type)
    {
        puComponent.powerUpType = type;
        puComponent.UpdateGraphics();
        powerUp.gameObject.SetActive(true);
        isPowerUp = true;
     
    }

    void Start()
    {
        SetBubbleSize(BubbleSize);
    }

    public void SetBubbleSize(int bubbleSize)
    {
        this.BubbleSize = bubbleSize;
        transform.localScale = Vector2.one;
        transform.localScale *= BubbleSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        _rb.AddForce(Vector2.up  * upSway.Evaluate(Time.deltaTime) * upSwayForce * Random.Range(0.1f, 1));

        _swayTimer -= Time.deltaTime;
        if(_swayTimer <= 0)
        {
            if(_rb.linearVelocityX > 0)
            {
                _rb.AddForce(Vector2.left * sideSwayForce * Random.Range(-0.1f ,1), ForceMode2D.Force);
            } else
            {
                _rb.AddForce(Vector2.right * sideSwayForce * Random.Range(-0.1f, 1), ForceMode2D.Force);
            }
           
            _swayTimer = Random.Range(swayTimer/3, swayTimer);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Projectile"))
        {
            OnDeath();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.instance.LoseLife();
            OnDeath();
        }
    }

    private void FreePowerUp()
    {
        if (isPowerUp)
        {
            powerUp.SetParent(transform.parent);
            powerUp.GetComponent<Collider2D>().enabled = true;
            powerUp.GetComponent<Rigidbody2D>().simulated = true;
            puComponent.enabled = true;   
        }
    }

    public void OnDeath()
    {
        if(BubbleSize <= 1)
        {
            AudioManager.instance.Play("bubble1");
            FreePowerUp();
            Destroy(this.gameObject);
        } else
        {
            AudioManager.instance.Play("bubble2");
            GameObject b1 = Instantiate(GameManager.instance.bubblePrefab, transform.position + new Vector3(0.5f, 0) , Quaternion.identity);
            b1.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, -0.1f) * Time.deltaTime * splitForce, ForceMode2D.Impulse);
            b1.GetComponent<BubbleLogic>().SetBubbleSize( Mathf.FloorToInt(this.BubbleSize / 2f));
         


            GameObject b2 = Instantiate(GameManager.instance.bubblePrefab, transform.position + new Vector3(0.5f, 0), Quaternion.identity);
            b2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, -0.1f) * Time.deltaTime *  splitForce, ForceMode2D.Impulse);
            b2.GetComponent<BubbleLogic>().SetBubbleSize(Mathf.FloorToInt(this.BubbleSize / 2f));



            if (isPowerUp)
            {
                float rand = Random.Range(0, 100);
                if(rand<= 50)
                {
                    b1.GetComponent<BubbleLogic>().SetPowerUp(puComponent.powerUpType);
                    b2.GetComponent<BubbleLogic>().powerUp.gameObject.SetActive(false);
                } else
                {
                    b2.GetComponent<BubbleLogic>().SetPowerUp(puComponent.powerUpType);
                    b1.GetComponent<BubbleLogic>().powerUp.gameObject.SetActive(false);
                }
            }

            Destroy(this.gameObject);
        }

    }
}
