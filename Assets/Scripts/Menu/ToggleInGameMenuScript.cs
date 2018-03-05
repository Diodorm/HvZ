using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInGameMenuScript : MonoBehaviour {

	GameObject inGameMenu;
	public int inGameMenuPos;
	// Use this for initialization
	void Start () {
		inGameMenu = this.gameObject.transform.GetChild (inGameMenuPos).gameObject;
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			inGameMenu.SetActive (!inGameMenu.gameObject.activeSelf);
			Time.timeScale = -(Time.timeScale) + 1;
		}
	}
}
