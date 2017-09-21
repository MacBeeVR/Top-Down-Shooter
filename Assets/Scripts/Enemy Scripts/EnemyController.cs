using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float speed;
    private float currentHP;
    [SerializeField]
    private int scoringWorth;
    private GameObject player;
    private Transform pTransform;
    private GameObject gcObject;
    private GameController gc;
    private ObjectPool objectPool;
    private NavMeshAgent nmAgent;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pTransform = player.transform;
        nmAgent = GetComponent<NavMeshAgent>();
        gcObject = GameObject.FindGameObjectWithTag("GameController");
        objectPool = gcObject.GetComponent<ObjectPool>();
        gc = gcObject.GetComponent<GameController>();
        nmAgent.speed = speed;
        nmAgent.acceleration = 200f;
        nmAgent.autoBraking = false;
    }

    //Sets the enemy to full health when the object is re-used.
    void OnEnable()
    {
        currentHP = maxHP;
    }

    //Makes the enemy look at the player.
    void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0f, 7.5f, 0f);

        //Move enemies while the game is still running.
        if (!gc.getGameOver())
        {
            //Moves enemy towards player using navmesh.
            nmAgent.destination = pTransform.position;
        }
        //Stop enemies when game over.
        else
        {
            nmAgent.ResetPath();
            nmAgent.SetDestination(gameObject.transform.localPosition);
        }
    }

    //This kills the enemy.
    private void die(int deathCode)
    {
        GameObject explosion = objectPool.getPooledItem("Explosion");
        if (explosion)
        {
            explosion.transform.position = transform.position;
            explosion.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        if (deathCode == 1)
            gc.addToScore(scoringWorth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            die(0);        
    }

    //Called by objects meant to damage enemies to apply damage.
    //If the enemy's HP is 0 or below, kill it.
	public void recieveDamage(float dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
            die(1);
    }
}
