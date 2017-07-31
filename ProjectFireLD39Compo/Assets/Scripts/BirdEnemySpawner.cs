using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BirdEnemySpawner : EnemySpawner {
    public float moveSpeedX = 5;


    public override void SpawnEnemy()
    {
        bool directionLeft = false;

        directionLeft = Random.Range(0, 2) == 0;

        Vector3 position = directionLeft ? TopRightCornerOfScreen : TopLeftCornerOfScreen;
        position.y -= Random.Range(2, 13);
        Bird bird = Instantiate(BirdPrefab, position, Quaternion.identity);
        bird.moveSpeedX = directionLeft ? -moveSpeedX : moveSpeedX;
        bird.GetComponent<SpriteRenderer>().flipX = directionLeft;
    }
    
    public abstract Bird BirdPrefab
    {
        get;
    }
}
