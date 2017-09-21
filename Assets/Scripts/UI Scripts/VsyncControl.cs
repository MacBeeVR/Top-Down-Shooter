using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VsyncControl : MonoBehaviour {

    private Button thisButton;
    private Text thisButtonText;

    //get reference to the button, text, and add a listener. Change text depending on value of vsync state.
	void Start()
    {
        thisButton = GetComponent<Button>();
        thisButtonText = GetComponentInChildren<Text>();
        thisButton.onClick.AddListener(ButtonClicked);
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync");
        changeButtonText();
    }

    //Change the vsync state.
    void ButtonClicked()
    {
        Debug.Log("VsyncButton Clicked");

        if (QualitySettings.vSyncCount == 1)
            QualitySettings.vSyncCount = 0;
        else
            QualitySettings.vSyncCount = 1;
            

        changeButtonText();
    }

    //Changes text based on vsync state.
    void changeButtonText()
    {
        if (QualitySettings.vSyncCount == 1)
            thisButtonText.text = "Disable VSync";
        if (QualitySettings.vSyncCount == 0)
            thisButtonText.text = "Enable VSync";

        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);
        PlayerPrefs.Save();
    }
}
