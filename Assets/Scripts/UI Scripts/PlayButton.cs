using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayButton : MonoBehaviour {

    //Declare reference for this button
    private Button thisButton;
    private Text thisButtonText;

    //Get reference to this button and add the click listener for it.
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ButtonClicked);
        thisButtonText = GetComponentInChildren<Text>();
    }

    //Load a scene. Put other scenes here later.
    void ButtonClicked()
    {
        SceneManager.LoadScene(thisButtonText.text);
    }
}
