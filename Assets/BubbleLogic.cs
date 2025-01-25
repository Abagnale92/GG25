using UnityEngine;

public class BubbleLogic : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _swayTimer;

    public float BubbleSize;
    public float upSwayForce;
    public float sideSwayForce;
    public AnimationCurve upSway;
    public float swayTimer;
    public float maxSideSpeed; 

    void Start()
    {
        this._rb = GetComponent<Rigidbody2D>();
        _swayTimer = swayTimer;
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
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
