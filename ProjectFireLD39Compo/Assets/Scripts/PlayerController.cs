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

    // Update is called once per frame
    void Update()
    {
        float dist =transform.position.y - ground.position.y;

        // left mouse button
        if(Input.GetMouseButtonDown(0))
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
        
        if (dist > 1.5f)
        {
            return;
        }
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") >= 0 ? Input.GetAxis("Vertical") : 0).normalized;
        float xTranslation = xDir * playerSpeed * Time.deltaTime;

        if(xTranslation > 0)
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
            Debug.Log("x: " + xTranslation * jumpStrength);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(xTranslation * jumpStrength * 10, jumpStrength), ForceMode2D.Impulse);
        }
    }
}
