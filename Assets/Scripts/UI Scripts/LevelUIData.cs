using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Contains a reference to each UI element in the LevelUI for access by the UIController.
public class LevelUIData : MonoBehaviour {

    public GameObject MainMenuButton;
    public GameObject QuitGameButton;
    public GameObject RestartButton;
    public GameObject PauseMenu;
    public GameObject SpawnPanel;
    public GameObject pickupPanel;
    public GameObject OptionsMenu;
    public Slider HealthBar;
    public Image HealthBarFill;
    public Text ScoreDisplay;
    public Text SurvivalTimeDisplay;
    public Text ItemSpawnText;
    public Text pickupText;

    void Awake()
    {
        pickupPanel.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }
}
