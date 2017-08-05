using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGoblinSpawner : EnemySpawner
{
    public float moveSpeedX = 5;
    public GreenGoblin greenGoblinPrefab;

    public override void SpawnEnemy()
    {
        bool directionLeft = false;

        directionLeft = Random.Range(0, 2) == 0;

        Vector3 position = directionLeft ? TopRightCornerOfScreen : TopLeftCornerOfScreen;
        position.y = -Random.Range(-10, 10);
        Bird bird = Instantiate(greenGoblinPrefab, position, Quaternion.identity);
        bird.moveSpeedX = moveSpeedX;
        bird.GetComponent<SpriteRenderer>().flipY = !directionLeft;
    }
}
