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
    public Arrow arrowPrefab;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public float lastJumpTimeStamp = 0;

    public bool mouseButtonDown = false;

    // Update is called once per frame
    void Update()
    {
        float dist =transform.position.y - ground.position.y;

        // left mouse button
        if(Input.GetMouseButtonDown(0))
        {
            ShootArrow();
            mouseButtonDown = true;
        }
        if (Input.GetMouseButton(0) && !mouseButtonDown)
        {
            mouseButtonDown = false;
            ShootArrow();
        }
        if (Input.GetMouseButtonUp(0) && !mouseButtonDown)
        {
            mouseButtonDown = false;
            ShootArrow();
        }
        
        if (dist > 1.5f)
        {
            return;
        }
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") >= 0 ? Input.GetAxis("Vertical") : 0).normalized;
        float xTranslation = xDir * playerSpeed * Time.deltaTime;

        if (rigidbody2D.velocity.x > -0.15 || rigidbody2D.velocity.x < 0.15)
        {
            xTranslation = xTranslation * 2f;
        }
        else if (rigidbody2D.velocity.x > -0.3 || rigidbody2D.velocity.x < 0.3)
        {
            xTranslation = xTranslation * 1.2f;
        }
        rigidbody2D.AddForce(new Vector2(xTranslation, 0), ForceMode2D.Impulse);  //makes player run


        if (rigidbody2D.velocity.x > maxSpeed || rigidbody2D.velocity.x < -maxSpeed)
        {
            rigidbody2D.velocity = ClampMagnitude(rigidbody2D.velocity, maxSpeed, -maxSpeed);
        }

        Debug.Log(rigidbody2D.velocity);

        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.gravityScale = 1;
        }
        else
        {
            rigidbody2D.gravityScale = 1.3f;
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

        if (yDir > 0 && Time.realtimeSinceStartup - lastJumpTimeStamp > 1)  //makes player jump
        {
            lastJumpTimeStamp = Time.realtimeSinceStartup;
            rigidbody2D.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }
    }

    private void ShootArrow()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 bowPosition = transform.GetChild(0).GetChild(0).position;
        Vector2 dir = point - bowPosition;
        Arrow arrow = Instantiate(arrowPrefab);
        arrow.transform.position = bowPosition;
        arrow.direction = dir.normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        GameManager.instance.PlayArrowShotSound();
    }

    private static Vector2 ClampMagnitude(Vector2 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }
}
