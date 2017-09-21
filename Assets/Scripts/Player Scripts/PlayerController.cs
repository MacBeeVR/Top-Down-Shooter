using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float maxHp;
    public float hp;
    [SerializeField]
    private float movementForce;
    [SerializeField]
    private float lookSpeed;
    [SerializeField]
    private float cameraFollowSpeed;
    [SerializeField]
    private Vector3 cameraOffset;
    private Vector3 velocity;
    private GameObject playerMeshObj;
    private FireController fController;
    private float standardFireRate;
    private bool invincible;
    private bool increasedFireRate;
    private float fireRateIncreaseEndTime;
    private float invincibilityEndTime;
    private GameController gc;
    private Rigidbody rb;
    private Camera cam;
    private Vector3 camPos;
    private Vector3 camRotation;
    private Vector3 moveDirection;
    private MeshRenderer playerMat;
    private Color normalMeshColor;
    private float h;
    private float v;

    //Initialize references at level load.
    void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        playerMat = GetComponent<MeshRenderer>();
        fController = GetComponent<FireController>();
        velocity = Vector3.one;
        cam = Camera.main;
        camPos = cam.transform.position;
        rb.freezeRotation = true;
        normalMeshColor = playerMat.material.color;
        standardFireRate = fController.fireRate;
        hp = maxHp;
        gc.UIControl.UpdateHealthDisplay(maxHp);
        fireRateIncreaseEndTime = Time.time;
        invincibilityEndTime = Time.time;
    }

    //Update handles the movement and the timing of powerups.
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        moveDirection = new Vector3(h, 0f, v);
        
        //If the invincibility interval has passed, disable invincibility and return player color to normal.
        if(invincible & Time.time > invincibilityEndTime)
        {
            invincible = false;
            playerMat.material.color = normalMeshColor;
            playerMat.material.SetColor("_EmissionColor", normalMeshColor * 1.7f);
        }
        //If the fire rate interval has passed, disable increased fire rate and go back to the standard fire rate.
        if (increasedFireRate & Time.time > fireRateIncreaseEndTime)
        {
            increasedFireRate = false;
            fController.fireRate = standardFireRate;
        }

        //Move player
        rb.AddForce((moveDirection.normalized * movementForce) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
    }

    //Smooth Camera Follow
    void LateUpdate()
    {
        Vector3 newCameraPosition = transform.position + cameraOffset;
        Vector3 cameraMove = Vector3.SmoothDamp(camPos, newCameraPosition, ref velocity, cameraFollowSpeed);
        camPos = cameraMove;
        cam.transform.position = camPos;
    }

    void OnTriggerEnter(Collider other)
    {
        //If enemy hits player when not invincible, do damage.
        if (other.gameObject.tag.Equals("Enemy") & !invincible)
            recieveDamage();
    }

    //Handles the player taking damage by reducing HP, changing color, and updating HP bar.
    public void recieveDamage()
    {
        hp -= 25;
        StartCoroutine(displayDamageTaken());
        gc.UIControl.UpdateHealthDisplay(hp);
        if (hp <= 0)
            die();
    }

    //Same as above but allows for a custom damage value.
    public void recieveDamage(int dmg)
    {
        hp -= dmg;
        StartCoroutine(displayDamageTaken());
        gc.UIControl.UpdateHealthDisplay(hp);
        if (hp <= 0)
            die();
    }

    //Restores health to max. Used by Health Restoration Powerup.
    public void restoreHealth()
    {
        hp = maxHp;
        gc.UIControl.UpdateHealthDisplay(hp);
    }

    //Increases firerate by .3 seconds. Used by Increase Fire Rate Powerup.
    public void IncreaseFireRate(float duration)
    {
        increasedFireRate = true;
        fireRateIncreaseEndTime = Time.time + duration;
        fController.fireRate = standardFireRate - .3f;
    }

    //Activates Invincibility and changes player color.
    public void activateInvincibility(float duration)
    {
        invincible = true;
        invincibilityEndTime = Time.time + duration;
        playerMat.material.color = Color.yellow;
        playerMat.material.SetColor("_EmissionColor", Color.yellow * 1.7f);
    }

    //this kills the player.
    public void die()
    {
        gameObject.SetActive(false);
    }

    //Coroutine for visually alerting the player that damage was received. Changes player color to red for .1 seconds.
    IEnumerator displayDamageTaken()
    {
        playerMat.material.color = Color.red;
        playerMat.material.SetColor("_EmissionColor", Color.red * 1.7f);
        yield return new WaitForSeconds(0.1f);
        playerMat.material.color = normalMeshColor;
        playerMat.material.SetColor("_EmissionColor", normalMeshColor * 1.7f);
    }
}
