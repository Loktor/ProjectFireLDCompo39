using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDir;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    public int jumpStrength = 10;
    public int maxSpeed = 13;
    public float playerSpeed;  //allows us to be able to change speed in Unity
    public Transform ground;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public float lastJumpTimeStamp = 0;


    // Update is called once per frame
    void Update()
    {
        float dist =transform.position.y - ground.position.y;

        if (dist > 1.5f)
        {
            return;
        }
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") >= 0 ? Input.GetAxis("Vertical") : 0).normalized;

        float xTranslation = xDir * playerSpeed * Time.deltaTime;
  
        if(rigidbody2D.velocity.y == 0) // in case of no jump damp velocity x faster
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * 0.8f, rigidbody2D.velocity.y);
        }

        if((rigidbody2D.velocity.x < 0 && xTranslation > 0) || (rigidbody2D.velocity.x > 0 && xTranslation < 0))
        {
            // set velocity to zero if we change direction
            rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
        }


        //if (rigidbody2D.velocity.x > maxSpeed || rigidbody2D.velocity.x < -maxSpeed)
        //{
        //    rigidbody2D.velocity = ClampMagnitude(rigidbody2D.velocity, maxSpeed, -maxSpeed);
        //}

        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.gravityScale = 1;
        }
        else
        {
            rigidbody2D.gravityScale = 1.5f;
        }

        if (!(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) 
            || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) 
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow)))
        {
            return;
        }

        if (xTranslation > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false; 
        }

        transform.Translate(xTranslation, 0f, 0f);  //makes player run

        if (yDir > 0 && Time.realtimeSinceStartup - lastJumpTimeStamp > 1)  //makes player jump
        {
            lastJumpTimeStamp = Time.realtimeSinceStartup;
            rigidbody2D.AddForce(new Vector2(xTranslation * 4.5f * jumpStrength, jumpStrength), ForceMode2D.Impulse);
        }
    }

    private static Vector2 ClampMagnitude(Vector2 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }
}
