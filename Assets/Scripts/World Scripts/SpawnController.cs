using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public float spawnRate;
    public float maxSpawnRate;
    public bool variableSpawn;
    public int percentChanceSpecial;
    public float spawnRateIncreaseInterval;
    public float spawnRateIncreaseIncrement;

    private ObjectPool objectPool;
    private GameObject gcObject;
    private GameController gc;
    private float nextSpawn;
    private float nextIncrease;

    void Awake()
    {
        nextSpawn = Time.time;
        nextIncrease = Time.time + spawnRateIncreaseInterval;
        gcObject = GameObject.FindGameObjectWithTag("GameController");
        gc = gcObject.GetComponent<GameController>();
        objectPool = gcObject.GetComponent<ObjectPool>();
    }

    void Update()
    {
        if (!gc.getGameOver())
        {
            //Checks if it's time to spawn another enemy and whether or not it should try to spawn a special.
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                if (variableSpawn)
                    rngSpawn();
                else
                    standardSpawn();
            }

            //Increases spawn rate as time goes on.
            if (Time.time > nextIncrease & spawnRate != maxSpawnRate)
            {
                nextIncrease = Time.time + spawnRateIncreaseInterval;
                spawnRate -= spawnRateIncreaseIncrement;
            }
        }
    }

    //Spawn Controller for Spawning Regular and Special Enemies.
    private void rngSpawn()
    {
        if (percentChanceSpecial != 0 & percentChanceSpecial != 100)
        {
            int randomNumber = Random.Range(0, 100);

            if (randomNumber < 100 - percentChanceSpecial)
                standardSpawn();
            else if (randomNumber > 100 - percentChanceSpecial)
                specialSpawn();
        }
        else if (percentChanceSpecial == 0)
            standardSpawn();
        else if (percentChanceSpecial == 100)
            specialSpawn();

    }

    //Spawn a regular enemy.
    private void standardSpawn()
    {
        GameObject newEnemy = objectPool.getPooledItem("Enemy");
        if (newEnemy)
        {
            newEnemy.transform.position = transform.position;
            newEnemy.transform.rotation = transform.rotation;
            newEnemy.SetActive(true);
        }
    }

    //Spawn a special enemy if one is available. Otherwise spawn a regular.
    private void specialSpawn()
    {
        GameObject newEnemy = objectPool.getPooledItem("SpecialEnemy");
        if (newEnemy)
        {
            newEnemy.transform.position = transform.position;
            newEnemy.transform.rotation = transform.rotation;
            newEnemy.SetActive(true);
        }
        else
            standardSpawn();
    }
}
