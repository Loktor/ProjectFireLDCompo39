using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerBird : Bird {

    public Bomb loggerBomb;
    public Log logPrefab; 

    public override void BirdDied()
    {
        base.BirdDied();

        Instantiate(logPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
    }

    protected override Bomb BombOnDeath
    {
        get
        {
            return loggerBomb;
        }
    }

    protected override void PlayDeathSound()
    {
        GameManager.instance.PlayLoggerEnemyDeathSound();
    }
}
