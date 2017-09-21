using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseController : MonoBehaviour {

    private GameObject pauseText;
    private GameObject deathText;

    //Initialize the different pause menu Text Values.
    void Awake()
    {
        pauseText = GameObject.Find("PauseText");
        deathText = GameObject.Find("DeathText");
        pauseText.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);
    }

    //Pause the game by enabling the pause menu hierarchy and setting timescale to 0
    //pause code 0 is standard pause. pause code 1 is death induced pause.
    public void PauseGame(int pauseCode)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        if(pauseCode == 0)
        {
            pauseText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if (pauseCode == 1)
            deathText.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }

    //disable pause menu and set timescale back to 1
    public void unPause()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        pauseText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    //invokes death induced pause.
    public void deathPause()
    {
        PauseGame(1);
    }
}
