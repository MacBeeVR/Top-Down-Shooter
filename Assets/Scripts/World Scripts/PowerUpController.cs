using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Handles the power up behaviours.
public class PowerUpController : MonoBehaviour {

    [SerializeField]
    private bool healthRestore;
    [SerializeField]
    private bool fireRateIncrease;
    [SerializeField]
    private bool invincible;
    [SerializeField]
    private bool Bomb;
    [SerializeField]
    private float fireRateIncreaseDuration;
    [SerializeField]
    private float invincibilityDuration;
    private float yPos;
    private PlayerController playerController;
    private GameController gameController;
    private ObjectPool pool;
    private bool canUse;
    private string powerUpName;

    //Gets references and starting y position.
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pool = gameController.gameObject.GetComponent<ObjectPool>();
        yPos = transform.position.y;
    }

    //Determines the type of power up this object is and displays the spawn text.
    void Start()
    {
        if (healthRestore)
            powerUpName = "Health Restore";
        if (fireRateIncrease)
            powerUpName = "Fire Rate Increase";
        if (invincible)
            powerUpName = "Invincibility ";
        if (Bomb)
            powerUpName = "Bomb";
        StartCoroutine(gameController.UIControl.ShowSpawnMessage(powerUpName));
    }

    //Allows the powerup to be used when it is enabled.
    void OnEnable()
    {
        canUse = true;
    }

    //Makes the power up float up and down and disables it if used.
    void Update()
    {
        transform.position = new Vector3(transform.position.x, yPos + 1f * Mathf.Sin(3f * Time.time), transform.position.z);

        if (!canUse)
            gameObject.SetActive(false);
    }

    //When the player enters the power up trigger give the player the appropriate advantage and display what was picked up.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") & canUse)
        {
            if (healthRestore)
                playerController.restoreHealth();
            if (fireRateIncrease)
                playerController.IncreaseFireRate(fireRateIncreaseDuration);
            if (invincible)
                playerController.activateInvincibility(invincibilityDuration);
            if (Bomb)
            {
                List<GameObject> activeEnemies = pool.getActiveObjects("Enemy");
                if (activeEnemies != null)
                {
                    for(int i = 0; i < activeEnemies.Count; i++)
                    {
                        activeEnemies[i].GetComponent<EnemyController>().recieveDamage(9999999);
                    }
                }
            }
            canUse = false;

            gameController.UIControl.ShowPickup(powerUpName);
        }
    }
}
