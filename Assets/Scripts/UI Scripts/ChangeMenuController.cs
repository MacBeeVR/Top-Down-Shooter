using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Handles menu changing in UI.
public class ChangeMenuController : MonoBehaviour {

    //Creating variables for the current menu, next menu and this button
    private Button thisButton;
    private Text thisButtonText;
    [SerializeField]
    private GameObject CurrentMenu;
    [SerializeField]
    private GameObject nextMenu;

    void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(buttonClicked);
        thisButtonText = GetComponentInChildren<Text>();
        
        //Disables the current menu this button is under if it is not the main menu.
        //main menu should never be disabled at start.
        if(GameObject.Find("Main Menu"))
        {
            if (CurrentMenu != GameObject.Find("Main Menu"))
                CurrentMenu.SetActive(false);
        }
    }

    //Enables the next menu to go to and disables this one.
    void buttonClicked()
    {
         nextMenu.SetActive(true);
         CurrentMenu.SetActive(false);
    }
}
