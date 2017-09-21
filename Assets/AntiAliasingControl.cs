using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AntiAliasingControl : MonoBehaviour {

    private Button thisButton;
    private Text thisButtonText;

	// Use this for initialization
	void Start () {
        thisButton = GetComponent<Button>();
        thisButtonText = GetComponentInChildren<Text>();
        thisButton.onClick.AddListener(ButtonClicked);
        QualitySettings.antiAliasing = PlayerPrefs.GetInt("AALevel");
        ChangeButtonText();
	}
	
    //Increments the Antialiasing level up to the next value when the button is clicked.
    //Also saves the new value and changes the text of the button to display the current AA Level.
	void ButtonClicked()
    {
        Debug.Log("AA Button Clicked");

        if (QualitySettings.antiAliasing == 0)
            QualitySettings.antiAliasing = 2;
        else if (QualitySettings.antiAliasing == 2)
            QualitySettings.antiAliasing = 4;
        else if (QualitySettings.antiAliasing == 4)
            QualitySettings.antiAliasing = 8;
        else if (QualitySettings.antiAliasing == 8)
            QualitySettings.antiAliasing = 0;

        PlayerPrefs.SetInt("AALevel", QualitySettings.antiAliasing);
        PlayerPrefs.Save();
        ChangeButtonText();
    }

    //Changes the Text of the button indicating level of Antialiasing.
    void ChangeButtonText()
    {
        thisButtonText.text = "AA Level: " + QualitySettings.antiAliasing;
    }
}
