using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGoblinSpawner : EnemySpawner
{
    public float moveSpeedX = 5;
    public GreenGoblin greenGoblinPrefab;

    public override void SpawnEnemy(Direction direction, int? yPos = null)
    {
        Vector3 position = direction == Direction.Left ? TopRightCornerOfScreen : TopLeftCornerOfScreen;
        if (yPos.HasValue)
        {
            position.y = yPos.Value;
        }
        else
        {
            position.y = Random.Range(-10, 5);
        }
        Bird bird = Instantiate(greenGoblinPrefab, position, Quaternion.identity);
        bird.moveSpeedX = moveSpeedX;
        bird.GetComponent<SpriteRenderer>().flipY = direction == Direction.Right;
    }
}
