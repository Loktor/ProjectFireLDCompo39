using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BirdEnemySpawner : EnemySpawner {
    public float moveSpeedX = 5;

    public override void SpawnEnemy(Direction direction, int? yPos = null)
    {
        Vector3 position = direction == Direction.Left ? TopRightCornerOfScreen : TopLeftCornerOfScreen;
        if(yPos.HasValue)
        {
            position.y = yPos.Value;
        }
        else
        {
            position.y = Random.Range(-5, 5);
        }
        Bird bird = Instantiate(BirdPrefab, position, Quaternion.identity);
        bird.moveSpeedX = direction == Direction.Left ? -moveSpeedX : moveSpeedX;
        bird.GetComponent<SpriteRenderer>().flipX = direction == Direction.Left;
    }
    
    public abstract Bird BirdPrefab
    {
        get;
    }
}
