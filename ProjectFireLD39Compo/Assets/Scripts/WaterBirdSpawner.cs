using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBirdSpawner : BirdEnemySpawner
{
    public WaterBird waterBirdPrefab;

    public override Bird BirdPrefab
    {
        get
        {
            return waterBirdPrefab;
        }
    }
}
