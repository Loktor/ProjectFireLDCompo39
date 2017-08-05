using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGoblin : Bird {
    public PoisonBomb poisonBombPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector2.MoveTowards(transform.position, GameManager.instance.player.transform.position, moveSpeedX / 60);
        var offset = new Vector2(transform.localPosition.x - GameManager.instance.player.transform.position.x, transform.localPosition.y - GameManager.instance.player.transform.position.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected override bool ShouldDestroyArrowOnHit()
    {
        return false;
    }

    protected override Bomb BombOnDeath
    {
        get
        {
            return poisonBombPrefab;
        }
    }

    protected override void PlayDeathSound()
    {
        GameManager.instance.PlayWaterEnemyDeathSound();
    }
}
