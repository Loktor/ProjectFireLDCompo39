using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerBirdSpawner : BirdEnemySpawner
{
    public LoggerBird loggerBirdPrefab;

    public override Bird BirdPrefab
    {
        get
        {
            return loggerBirdPrefab;
        }
    }
}
