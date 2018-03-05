// Team Unity Lunacy
// Leo Chen, Daniel Kane, Rayner Kristanto, Frank Marzen, Lixin Wang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardMenuController : MonoBehaviour {

	Button beginButton;
	Button creditsButton;
	bool keyboardEnabled = false;

	// Use this for initialization
	void Start () {
		beginButton = this.gameObject.transform.GetChild (2).gameObject.GetComponent<Button>();
		creditsButton = this.gameObject.transform.GetChild (3).gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!keyboardEnabled && (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow))) {
			keyboardEnabled = true;
			beginButton.Select ();
			// Unity's navigation system then processes the user input and selects the begin button
		}
	}

}
