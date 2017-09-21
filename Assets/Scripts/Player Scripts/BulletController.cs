using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    //Values for bullet range and damage.
    //These values are defaulted to 20 and 5 in Unity.
    //Values can be modified in the player's Fire Controller.
    public float range;
    public float damage;

    private ObjectPool objectPool;
    private Vector3 startingPoint;
    private MeshRenderer mesh;
    private Rigidbody rb;
    private AudioSource fireSound;
    private bool stopped;


    //Deactivates the bullet allowing it to be used again from the pool.
    //If the bullet hit something, it will play a hit particle effect.
    void deactivate(int deactivationCode)
    {
        if(deactivationCode == 1)
        {
            GameObject hit = objectPool.getPooledItem("BulletHit");
            hit.transform.position = transform.position;
            hit.transform.rotation = transform.rotation;
            hit.SetActive(true);
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (mesh.enabled == false)
            mesh.enabled = true;
        stopped = false;
        gameObject.SetActive(false);
    }

    void Awake()
    {
        //Get Reference to the Bullet's rigid body on creation
        rb = GetComponent<Rigidbody>();
        objectPool = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectPool>();
        fireSound = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
        stopped = false;
    }
        
    void OnEnable()
    {
        //Get firing position and make the bullet active after being fired.
        startingPoint = transform.position;
        fireSound.Play();
    }
        
    void Update()
    {
        //Check if bullet has travelled beyond it's max range and disables it if so.
        if (Vector3.Distance(startingPoint, transform.position) > range)
            deactivate(0); 
    }

    void FixedUpdate()
    {
        //Makes the bullet go forward if it is currently active.
        if (gameObject.activeInHierarchy & !stopped)
            rb.velocity = (transform.forward * 75f);
    }


    //When the bullet hits something, apply damage if it's an enemy or just explode if it is anything else other than another bullet or powerup.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().recieveDamage(damage);
            if (fireSound.isPlaying)
            {
                stopped = true;
                StartCoroutine(DestroyAfterSound());
            }
            else    
                deactivate(1);
        }
        else if (!other.gameObject.tag.Equals("Bullet") & !other.gameObject.tag.Equals("Powerup"))
        {
            if (fireSound.isPlaying)
            {
                stopped = true;
                StartCoroutine(DestroyAfterSound());
            }
            else
                deactivate(1);
        }
    }

    IEnumerator DestroyAfterSound()
    {
        mesh.enabled = false;
        yield return new WaitUntil(() => !fireSound.isPlaying);
        deactivate(1);
    }
}
