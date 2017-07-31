using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bird : MonoBehaviour {
    public float moveSpeedX;
    public int hp = 1;

    protected virtual Bomb BombOnDeath
    {
        get
        {
            return null;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(new Vector2(moveSpeedX / 100, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            HitByBullet();
            if(!IsAlive())
            {
                BirdDied();
            }
            Destroy(collision.gameObject);
        }
    }

    protected virtual void HitByBullet()
    {
        hp -= 1;
    }
    
    protected virtual bool IsAlive()
    {
        return hp > 0;
    }

    protected virtual void BirdDied()
    {
        if (BombOnDeath)
        {
            Bomb bomb = Instantiate(BombOnDeath, this.transform.position, Quaternion.identity);
            bomb.GetComponent<ParticleSystem>().Play();
        }
        PlayDeathSound();
        Destroy(gameObject);
    }

    protected abstract void PlayDeathSound();
}
