using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField]
    private LevelUIData UIData;
    private GameController gameController;

    private float disablePickupPanelTime;

    //Get gameController reference and LevelUIData.
    void Awake()
    {
        UIData = GameObject.Find("Level UI").GetComponent<LevelUIData>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (UIData.SpawnPanel.activeInHierarchy)
            UIData.SpawnPanel.SetActive(false);
    }


    void Update()
    {
        //Updates the time as long as the game is not yet over. If it's over, display the death screen.
        if (!gameController.getGameOver())
        {
            UIData.SurvivalTimeDisplay.text = Time.timeSinceLevelLoad.ToString("0.00");

            if (UIData.pickupPanel.activeInHierarchy & Time.time > disablePickupPanelTime)
                UIData.pickupPanel.SetActive(false);
        }
        else
            ShowDeathScreen();
    }

    #region UITextDisplayItems
    //Updates Health Display and Color of health bar.
    public void UpdateHealthDisplay(float hp)
    {
        if(!UIData)
            Debug.Log("UIData Null");
        UIData.HealthBar.value = hp;

        if (UIData.HealthBar.value > (UIData.HealthBar.maxValue * .7f))
            UIData.HealthBarFill.color = Color.green;
        if (UIData.HealthBar.value < (UIData.HealthBar.maxValue * .7f))
            UIData.HealthBarFill.color = Color.yellow;
        if (UIData.HealthBar.value < (UIData.HealthBar.maxValue * .3f))
            UIData.HealthBarFill.color = Color.red;
        if (UIData.HealthBar.value <= UIData.HealthBar.minValue)
            UIData.HealthBarFill.enabled = false;
    }

    //Updates the score display.
    public void UpdateScoreDisplay(int score)
    {
        UIData.ScoreDisplay.text = score.ToString();
    }

    //Shows the spawn message for 2 seconds.
    public IEnumerator ShowSpawnMessage(string spawnedItemName)
    {
        UIData.ItemSpawnText.text = spawnedItemName + " has Spawned!";
        UIData.SpawnPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIData.SpawnPanel.SetActive(false);
    }

    //Shows what PowerUp was picked up for 2 seconds.
    public void ShowPickup(string spawnedItemName)
    {
        UIData.pickupPanel.SetActive(true);
        UIData.pickupText.text = "Picked Up: " + spawnedItemName;
        disablePickupPanelTime = Time.time + 2f;
    }
    #endregion

    #region PauseMenuItems
    //Shows death screen after death.
    public void ShowDeathScreen()
    {
        UIData.PauseMenu.SetActive(true);
        UIData.PauseMenu.GetComponent<PauseController>().deathPause();
    }

    //Pause the game using the pause function from the pause controller.
	public void PauseGame()
    {
        UIData.PauseMenu.SetActive(true);
        UIData.PauseMenu.GetComponent<PauseController>().PauseGame(0);
    }

    //Unpause the game using the unpause function from the pause controller.
    public void UnpauseGame()
    {
        if (UIData.PauseMenu.activeInHierarchy)
            UIData.PauseMenu.GetComponent<PauseController>().unPause();
    }

    //Returns whether or not the player is viewing the options menu. Pauses the ability to unpause.
    public bool InOptionsMenu()
    {
        return UIData.OptionsMenu.activeInHierarchy;
    }
    #endregion
}
