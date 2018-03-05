// Team Unity Lunacy
// Leo Chen, Daniel Kane, Rayner Kristanto, Frank Marzen, Lixin Wang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionSceneScript : MonoBehaviour {

	public string scene;
	// for buttons
	Button button;

	// Use this for initialization
	void Start () {
		button = this.gameObject.GetComponent<Button> ();
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		SceneManager.LoadScene (scene);
	}
}
