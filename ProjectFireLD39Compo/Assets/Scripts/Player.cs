using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Arrow arrowPrefab;
    public bool mouseButtonDown = false;
    public GameObject bowSprite;
    public Color poisonedBowColor;
    private float poisonStartTimeInSeconds = -1;
    private bool poisoned = false;
    private Renderer bowRenderer;
    private Color initialBowColor;

    // Use this for initialization
    void Start ()
    {
        bowRenderer = bowSprite.GetComponent<Renderer>();
        initialBowColor = bowRenderer.material.GetColor("_EmisColor");
    }
	
	// Update is called once per frame
	void Update () {
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle + 225);

        // left mouse button
        if (Input.GetMouseButtonDown(0))
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.LogCollected();
        }
        if (collision.gameObject.CompareTag("GreenGoblin"))
        {
            (collision.gameObject.GetComponent<GreenGoblin>()).BirdDied();
            ChangeToPoisonedState();
        }
    }
    
    private void ShootArrow(float angleOffset = 0)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 bowPosition = transform.GetChild(0).GetChild(0).position;
        Vector2 dir = (point - bowPosition + new Vector3(angleOffset, angleOffset, 0)); 
        Arrow arrow = Instantiate(arrowPrefab);
        arrow.transform.position = bowPosition;
        arrow.direction = dir.normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        if(Time.realtimeSinceStartup - poisonStartTimeInSeconds < 5 && poisoned)
        {
            arrow.speed = arrow.speed / 4;
            arrow.GetComponentInChildren<Light>().color = Color.green;
            arrow.GetComponentInChildren<Light>().intensity = 3;
        }
        else
        {
            EndPoisonedState();
        }
        GameManager.instance.PlayArrowShotSound();
    }

    private void ChangeToPoisonedState()
    {
        poisonStartTimeInSeconds = Time.realtimeSinceStartup;
        poisoned = true;
        ChangeBowColor(poisonedBowColor);
    }

    private void EndPoisonedState()
    {   
        poisoned = false;
        ChangeBowColor(initialBowColor);
    }

    private void ChangeBowColor(Color color)
    {
        Material mat = bowRenderer.material;
        mat.SetColor("_EmisColor", color);
    }
}
