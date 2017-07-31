using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : MonoBehaviour {

    bool moveToStartLocation = false;
    bool kindling = false;
    public Vector2 startTarget = new Vector2(0, 5);
    bool growing = false;
    public bool collectionAllowed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(moveToStartLocation)
        {
            if(((Vector2)this.transform.position) == startTarget)
            {
                moveToStartLocation = false;
                kindling = true;
                growing = true;
                GameManager.instance.OrbInPosition();
            }
            this.transform.position = Vector2.MoveTowards(this.transform.position, startTarget, 0.05f);
        }
        if(kindling)
        {
            if(growing)
            {
                if(this.transform.localScale.x > 1f)
                {
                    GameManager.instance.PlayFireOrbSound();
                    growing = false;
                }
                this.transform.localScale *= 1.02f;
            }
            else
            {
                if(this.transform.localScale.x < 0.5f)
                {
                    GameManager.instance.PlayFireOrbSound();
                    growing = true;
                }
                this.transform.localScale *= 0.98f;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collectionAllowed)
        {
            moveToStartLocation = true;
            GameManager.instance.PlayFireOrbFlowingSound();
        }
        if (kindling && collision.gameObject.CompareTag("Arrow"))
        {
            this.transform.localScale *= 0.2f;
            kindling = false;
            GetComponentInChildren<Light>().intensity = 0;
            GetComponent<ParticleSystem>().Play();
            GameManager.instance.PlayFireOrbBreakingSound();
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.StartGame();
        Destroy(gameObject);
    }
}
