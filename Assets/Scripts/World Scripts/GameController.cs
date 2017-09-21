using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    private bool PowerUps;
    [SerializeField]
    private bool showFrameRate;
    private GameObject player;
    private PlayerController pControl;
    private Image healthBarImage;
    private Text fpsCounter;
    private int score;
    private float frameRateUpdate;
    private bool gameOver;
    private bool gamePaused;
    private string mapName;
    
    //UIController Reference for Use by objects when modifying UI.
    public UIController UIControl;

    //Initialize references to various things in the game that need to be accessed and setting initial timeScale to 1.
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pControl = player.GetComponent<PlayerController>();
        gameOver = false;
        score = 0;
        Time.timeScale = 1;
        fpsCounter = GameObject.Find("FPSCounter").GetComponent<Text>();
        GetComponent<PowerUpNavMeshSpawner>().enabled = PowerUps;
        frameRateUpdate = Time.time;

        if (!showFrameRate)
            fpsCounter.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameOver)
        {
            //If/Else invokes pause menu via the pause controller script.
            if (Input.GetKeyDown(KeyCode.Escape) & !gamePaused)
            {
                UIControl.PauseGame();
                gamePaused = true;
            }    
            else if (Input.GetKeyDown(KeyCode.Escape) & !UIControl.InOptionsMenu())
            {
                UIControl.UnpauseGame();
                gamePaused = false;
            }
                
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Cursor.lockState = CursorLockMode.Confined;

            //Keeps a check on whether or not the player character has died.
            if (!pControl.gameObject.activeInHierarchy)
                setGameOver();

            //Enable/Disable the FrameRate Counter when P is pressed.
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (showFrameRate)
                {
                    showFrameRate = false;
                    fpsCounter.gameObject.SetActive(false);
                }
                else
                {
                    showFrameRate = true;
                    fpsCounter.gameObject.SetActive(true);
                }
            }
        }

        //Updates the currentFramerate after the wait interval has passed.
        if (showFrameRate & Time.time > frameRateUpdate)
            UpdateFramerate();

        //Pause game if the player tabs out.
        if (!Application.isFocused)
            UIControl.PauseGame();
    }

    //Updates the framerate counter and sets the next time to update.
    private void UpdateFramerate()
    {
        frameRateUpdate = Time.time + .6f;
        fpsCounter.text = ((int)(1 / Time.unscaledDeltaTime)).ToString() + " FPS";
    }

    //Ends the game and opens the death invoked pause menu via the pause controller script.
    public void setGameOver()
    {
        gameOver = true;
    }

    //Reloads the current level. Called by the restart level button on the pause menu.
    public void restartLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevel);
    }

    //returns whether of not the game is over.
    public bool getGameOver()
    {
        return gameOver;
    }

    public bool Paused()
    {
        return gamePaused;
    }

    //adds the enemy score worth to the score. Updates the score text via the UI Controller.
    public void addToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UIControl.UpdateScoreDisplay(score);
    }
}
