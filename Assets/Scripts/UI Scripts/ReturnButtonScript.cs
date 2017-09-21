using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ReturnButtonScript : MonoBehaviour {

    private Button thisButton;

    //Get reference to this button and add a click listener.
	void OnEnable()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ButtonClicked);
    }

    //load main menu when clicked.
    void ButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
