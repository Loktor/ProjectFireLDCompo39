using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour {

    public float spawningProbabilityPercentage = 1;
    public bool active = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(active && Random.Range(0f, 100f) < spawningProbabilityPercentage)
        {
            SpawnEnemy();
        }
	}

    public Vector2 TopRightCornerOfScreen
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            return Camera.main.ViewportToWorldPoint(topRightCorner);
        }
    }

    public Vector2 TopLeftCornerOfScreen
    {
        get
        {
            Vector2 topLeftCorner = new Vector2(0, 1);
            return Camera.main.ViewportToWorldPoint(topLeftCorner);
        }
    }

    public abstract void SpawnEnemy();
}
