using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartButton : MonoBehaviour {

    private Button thisButton;

    //Get reference to this button and add the click listener.
    void OnEnable()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ButtonClicked);
    }

    //Reload the current scene when the restart button is clicked.
    void ButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
