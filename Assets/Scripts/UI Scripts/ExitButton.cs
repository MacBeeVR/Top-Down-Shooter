using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    private Button thisButton;

	void Start () {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ButtonClicked);
	}
	
	void ButtonClicked()
    {
        Application.Quit();
    }
}
