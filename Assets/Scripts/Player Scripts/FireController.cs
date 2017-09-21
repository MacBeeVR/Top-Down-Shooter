using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

    //Values for FireRate (in milliseconds between shots) and range.
    public float fireRate;
    public float gunRange;
    public float gunDamage;
    //References to the bullet spawn point on the player and the game controller object.
    public GameObject bulletExit;
    
    private float timeToNextShot;
    private ObjectPool objectPool;
    private GameObject gcObject;
    private GameController gControl;
    private bool canShoot;

    void Awake()
    {
        //set the the next shot available now and get the bulletpool script reference.
        timeToNextShot = Time.time;
        gcObject = GameObject.FindGameObjectWithTag("GameController");
        gControl = gcObject.GetComponent<GameController>();
        objectPool = gcObject.GetComponent<ObjectPool>();
        canShoot = false;
    }

    void Update()
    {
        //If the game is not paused
        if (!gControl.Paused())
        {
            //if left mouse is held, fire the gun from the bulletspawn (bulletExit)
            if (Input.GetKey(KeyCode.Mouse0) & Time.time > timeToNextShot)
            {
                canShoot = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) & canShoot)
            {
                canShoot = false;
            }
        }
    }

    //Fixed Update handles the actual firing for even bullet firing.
    void FixedUpdate()
    {
        //If the fire key is pressed and the wait interval for firing has passed, fire.
        if (canShoot & Time.time > timeToNextShot)
        {
            //sets the next time to fire.
            timeToNextShot = Time.time + fireRate;

            //Gets an available pooled bullet object. If an object exists, enable and shoot it.
            GameObject newBullet = objectPool.getPooledItem("Bullet");
            if (newBullet)
            {
                newBullet.transform.position = bulletExit.transform.position;
                newBullet.transform.rotation = bulletExit.transform.rotation;
                newBullet.GetComponent<BulletController>().range = gunRange;
                newBullet.GetComponent<BulletController>().damage = gunDamage;
                newBullet.SetActive(true);
            }
        }
    }
}
