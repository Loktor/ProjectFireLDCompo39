using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBird : Bird {
    int waterBombThrownAmount = 0;

    public WaterBomb waterBombPrefab;
    private Light light;

    // Use this for initialization
    protected override void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(moveSpeedX > 0 ? this.transform.position.x < 0 : this.transform.position.x > 0)
        {
            base.Update();
        }
        else
        {
            this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
            if (waterBombThrownAmount < 5)
            {
                if (Random.Range(0, 50) > 2)
                {
                    waterBombThrownAmount++;
                    transform.localScale *= 1.1f;
                    light.range *= 1.1f;
                    light.intensity *= 1.1f;
                }
            }
            else if(waterBombThrownAmount >= 5 && waterBombThrownAmount < 6)
            {
                 waterBombThrownAmount++;
                 WaterBomb waterBomb = Instantiate(waterBombPrefab, this.transform.position, Quaternion.identity);
                waterBomb.GetComponent<ParticleSystem>().Play();

            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(WaterBombThrown());
            }
        }
    }

    protected IEnumerator WaterBombThrown()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.BombExplodedOverFire();
        Destroy(gameObject);
    }

    protected override Bomb BombOnDeath
    {
        get
        {
            return waterBombPrefab;
        }
    }

    protected override void PlayDeathSound()
    {
        GameManager.instance.PlayWaterEnemyDeathSound();
    }
}
