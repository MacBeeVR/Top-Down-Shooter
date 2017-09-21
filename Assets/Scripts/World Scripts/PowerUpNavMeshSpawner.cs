using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

//Handles spawning of powerups in various locations within the navmeshed map.

public class PowerUpNavMeshSpawner : MonoBehaviour {

    //Class for each pooled PowerUp
    [System.Serializable]
    public class PowerUp
    {
        public GameObject powerUp;
        public GameObject hierarchy;
        public string powerUpType;
        public bool resizable;
        public int numToPool;

        [System.NonSerialized]
        public List<GameObject> pool;

        //Builds the pool for this specific powerup type.
        public void BuildPool()
        {
            pool = new List<GameObject>();
            for(int i = 0; i < numToPool; i++)
            {
                GameObject newObj = (GameObject)Instantiate<GameObject>(powerUp);
                newObj.transform.parent = hierarchy.transform;
                newObj.SetActive(false);
                pool.Add(newObj);
            }
        }

        //Returns an available powerup or returns a newly created powerup if needed and specified.
        public GameObject getPowerUp()
        {
            for(int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                    return pool[i];
            }

            if (resizable)
                return resizePool();
            else
                return null;
        }

        //Adds to the pool and returns the newly created powerup.
        public GameObject resizePool()
        {
            GameObject newObj = (GameObject)Instantiate<GameObject>(powerUp);
            newObj.transform.parent = hierarchy.transform.parent;
            newObj.SetActive(false);
            pool.Add(newObj);
            return newObj;
        }
    }

    public List<PowerUp> powerUpPool;

    [SerializeField]
    private float spawnDuration;
    private float nextSpawn;
    private GameController gameController;

    //Get GameController Reference, and set the spawntime to whatever the duration is later.
    void Awake()
    {
        nextSpawn = Time.time + spawnDuration;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        for(int i = 0; i < powerUpPool.Count; i++)
        {
            powerUpPool[i].BuildPool();
        }
    }


    void Update()
    {
        //While the game is in progress, spawn one of the powerups at a random point on the map every so many seconds.
        if (!gameController.getGameOver())
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnDuration;
                GameObject newPowerUp = powerUpPool[Random.Range(0, powerUpPool.Count)].getPowerUp();
                if (newPowerUp)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(new Vector3(Random.Range(-250f, 250f), 0f, Random.Range(-250f, 250f)), out hit, 450f, 1))
                        newPowerUp.transform.position = hit.position;

                    newPowerUp.SetActive(true);
                }
            }
        }
    }
}
