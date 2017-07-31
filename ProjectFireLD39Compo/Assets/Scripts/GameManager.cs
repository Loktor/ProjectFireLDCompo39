﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public  class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Fire fire;
    public FireOrb fireOrb;
    public Player player;
    private bool gameRunning = false;
    public LoggerBirdSpawner loggerSpawner;
    public WaterBirdSpawner waterSpawner;
    public Bomb winningBombsPrefab;
    public Text titleText;
    public Text infoText;
    public Text gameDurationText;
    public AudioClip growingOrb;
    public AudioClip breakingOrb;
    public AudioClip fireStarting;
    public AudioClip fireHit;
    public AudioClip orbFlowing;
    public List<AudioClip> arrowShotSounds = new List<AudioClip>();
    public List<AudioClip> waterEnemyDeathSounds = new List<AudioClip>();
    public List<AudioClip> loggerEnemyDeathSounds = new List<AudioClip>();
    public List<AudioClip> logHitFloorSounds = new List<AudioClip>();
    private bool gameOver = false;
    private float gameTimeInSeconds = -1;
    private float gameStartTimeInSeconds = -1;
    private bool initialLoggerSpawned = false;
    private int initialWaterBirdSpawnedCount = 0;
    private bool uberWaveSpawned = false;
    private bool gameWon = false;
    private int oldGameTime = 0;

    public bool GameRunning
    {
        get
        {
            return gameRunning;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        oldGameTime = 0;
        gameWon = false;
        uberWaveSpawned = false;
        gameOver = false;
        gameTimeInSeconds = -1;
        gameStartTimeInSeconds = -1;
        initialLoggerSpawned = false;
        initialWaterBirdSpawnedCount = 0;
        StartCoroutine(StartOrbVisibility());
    }

    private IEnumerator StartOrbVisibility()
    {
        yield return new WaitForSeconds(3);
        PlayFireOrbSound();
        fireOrb.GetComponentInChildren<Light>().intensity = 5;
        yield return new WaitForSeconds(1);
        PlayFireOrbSound();
        fireOrb.GetComponentInChildren<Light>().intensity = 10;
        yield return new WaitForSeconds(1);
        PlayFireOrbSound();
        fireOrb.GetComponentInChildren<Light>().intensity = 20;
        infoText.text = "Collect the orb(WASD to move)";
        fireOrb.collectionAllowed = true;
    }

    public void PlayFireOrbSound()
    {
        AudioSource.PlayClipAtPoint(growingOrb, transform.position);
    }

    public void PlayFireOrbFlowingSound()
    {
        AudioSource.PlayClipAtPoint(orbFlowing, transform.position);
    }

    public void PlayFireOrbBreakingSound()
    {
        AudioSource.PlayClipAtPoint(breakingOrb, transform.position);
    }

    public void PlayFireStartingSound()
    {
        AudioSource.PlayClipAtPoint(fireStarting, transform.position);
    }

    public void PlayFireHitSound()
    {
        AudioSource.PlayClipAtPoint(fireHit, transform.position);
    }

    public void PlayArrowShotSound()
    {
        AudioSource.PlayClipAtPoint(arrowShotSounds[Random.Range(0, arrowShotSounds.Count)], transform.position);
    }

    public void PlayWaterEnemyDeathSound()
    {
        AudioSource.PlayClipAtPoint(waterEnemyDeathSounds[Random.Range(0, waterEnemyDeathSounds.Count)], transform.position);
    }

    public void PlayLoggerEnemyDeathSound()
    {
        AudioSource.PlayClipAtPoint(loggerEnemyDeathSounds[Random.Range(0, loggerEnemyDeathSounds.Count)], transform.position);
    }

    public void PlayLogHitFloorSound()
    {
        AudioSource.PlayClipAtPoint(logHitFloorSounds[Random.Range(0, logHitFloorSounds.Count)], transform.position);
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update () {
        if (gameRunning)
        {
            gameTimeInSeconds = Time.realtimeSinceStartup - gameStartTimeInSeconds;
            gameDurationText.text = "Game Duration: " + (int)gameTimeInSeconds + "s";
        }
        if (gameOver && Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (fire.fireHealth <= 0)
        {
            gameOver = true;
            GameOver();
            return;
        }

        if (gameTimeInSeconds > 500)
        {
            loggerSpawner.active = false;
            waterSpawner.active = false;
            if (!gameWon)
            {
                GameWon();
            }
            if (Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (((int)gameTimeInSeconds) != oldGameTime)
            {
                oldGameTime = (int)gameTimeInSeconds;
                PlayWinnerBombs();
            }
            return;
        }

        if (!gameRunning)
        {
            return;
        }       

        if (gameTimeInSeconds > 5 && gameTimeInSeconds < 7 && !initialLoggerSpawned)
        {
            initialLoggerSpawned = true;
            loggerSpawner.moveSpeedX = 2;
            loggerSpawner.SpawnEnemy();
        }

        if(gameTimeInSeconds > 5 && gameTimeInSeconds < 20)
        {
            infoText.text = "Shoot the red spirit and collect his essence to increase the flame";
        }
        else if (gameTimeInSeconds > 20 && gameTimeInSeconds < 30)
        {
            infoText.text = "Shoot the blue spirit before he reaches the flame";
        }
        else if (gameTimeInSeconds > 90 && gameTimeInSeconds < 100)
        {
            InitUberWave();
            if (gameTimeInSeconds > 93)
            {
                SpawnUberWave(6, loggerSpawner, "a Wave!!!");
            }
            return;
        }
        else if (gameTimeInSeconds > 120 && gameTimeInSeconds < 130)
        {
            InitUberWave();
            if (gameTimeInSeconds > 123)
            {
                SpawnUberWave(12, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 160 && gameTimeInSeconds < 170)
        {
            InitUberWave();
            if (gameTimeInSeconds > 123)
            {
                SpawnUberWave(20, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 200 && gameTimeInSeconds < 210)
        {
            InitUberWave();
            if (gameTimeInSeconds > 203)
            {
                SpawnUberWave(20, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 280 && gameTimeInSeconds < 290)
        {
            InitUberWave();
            if (gameTimeInSeconds > 283)
            {
                SpawnUberWave(20, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 320 && gameTimeInSeconds < 330)
        {
            InitUberWave();
            if (gameTimeInSeconds > 323)
            {
                SpawnUberWave(20, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 350 && gameTimeInSeconds < 360)
        {
            InitUberWave();
            if (gameTimeInSeconds > 353)
            {
                SpawnUberWave(20, loggerSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 420 && gameTimeInSeconds < 430)
        {
            InitUberWave();
            if (gameTimeInSeconds > 423)
            {
                SpawnUberWave(10, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 470 && gameTimeInSeconds < 478)
        {
            InitUberWave();
            if (gameTimeInSeconds > 473)
            {
                SpawnUberWave(10, waterSpawner);
            }
            return;
        }
        else if (gameTimeInSeconds > 490 && gameTimeInSeconds < 500)
        {
            InitUberWave();
            if (gameTimeInSeconds > 493)
            {
                SpawnUberWave(20, waterSpawner);
            }
            return;
        }
        else if(gameTimeInSeconds > 30 && gameTimeInSeconds < 60)
        {
            infoText.gameObject.SetActive(false);
        }
        else if (gameTimeInSeconds > 60)
        {
            loggerSpawner.active = true;
            waterSpawner.active = true;
            infoText.gameObject.SetActive(false);
            uberWaveSpawned = false;
        }

        if (gameTimeInSeconds > 20 && initialWaterBirdSpawnedCount < 1)
        {
            initialWaterBirdSpawnedCount++;
            waterSpawner.SpawnEnemy();
        }

        if (gameTimeInSeconds > 30 && initialWaterBirdSpawnedCount < 2)
        {
            initialWaterBirdSpawnedCount++;
            loggerSpawner.moveSpeedX = 6;
            loggerSpawner.SpawnEnemy();
            waterSpawner.SpawnEnemy();
        }

        if (gameTimeInSeconds > 40)
        {
            loggerSpawner.active = true;
        }
        if (gameTimeInSeconds > 60)
        {
            waterSpawner.active = true;
        }

        if (gameTimeInSeconds > 200)
        {
            loggerSpawner.moveSpeedX = 11;
        }

        if (gameTimeInSeconds > 400)
        {
            waterSpawner.moveSpeedX = 11;
        }
        else if (gameTimeInSeconds > 240)
        {
            waterSpawner.moveSpeedX = 9;
        }
        else if (gameTimeInSeconds > 180)
        {
            waterSpawner.moveSpeedX = 8;
        }
        else if (gameTimeInSeconds > 100)
        {
            waterSpawner.moveSpeedX = 7;
        }
	}

    private void InitUberWave()
    {
        loggerSpawner.active = false;
        waterSpawner.active = false;
        infoText.gameObject.SetActive(true);
        infoText.text = "Something is approaching!!!";
        return;
    }

    private void SpawnUberWave(int amount, EnemySpawner spawner, string text = "Uber WAVE!!!")
    {
        infoText.gameObject.SetActive(true);
        infoText.text = text;
        if (!uberWaveSpawned)
        {
            loggerSpawner.active = false;
            waterSpawner.active = false;
            for (int i = 0; i < amount; i++)
            {
                spawner.SpawnEnemy();
            }
            uberWaveSpawned = true;
        }
        return;
    }

    private void GameWon()
    {
        gameWon = true;
        titleText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
        titleText.text = "You kept the flame alive, Congratulations";
        infoText.text = "Press Space to restart";
    }

    private void GameOver()
    {
        gameRunning = false;
        titleText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
        titleText.text = "Game Over";
        infoText.text = "Press Space to restart";
    }

    public void OrbInPosition()
    {
        infoText.text = "Shoot the orb to rekindle the last flame (left mouse button)";
    }

    public void StartGame()
    {
        titleText.gameObject.SetActive(false);
        gameDurationText.gameObject.SetActive(true);
        infoText.text = "Keep the flame alive!";
        fire.IncreaseSize(100);
        gameRunning = true;
        gameStartTimeInSeconds = Time.realtimeSinceStartup;
        PlayFireStartingSound();
    }

    public void PlayWinnerBombs()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
            screenPosition.z = 0;
            Bomb winnerBomb = Instantiate(winningBombsPrefab, screenPosition, Quaternion.identity);
            winnerBomb.GetComponent<ParticleSystem>().Play();
        }
    }

    public void LogCollected()
    {
        fire.IncreaseSize(5);
        PlayFireStartingSound();
    }

    public void BombExplodedOverFire()
    {
        fire.DecreaseSize(25);
        PlayFireHitSound();
    }
}
