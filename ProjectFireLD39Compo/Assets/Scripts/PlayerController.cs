using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector2 moveDir;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    public int jumpStrength = 10;
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

        if(!(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(xTranslation * jumpStrength * 10, jumpStrength), ForceMode2D.Impulse);
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
}
