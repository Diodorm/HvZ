// Team Unity Lunacy
// Leo Chen, Daniel Kane, Rayner Kristanto, Frank Marzen, Lixin Wang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedScript : MonoBehaviour {

	public string animName;
	public float speed;
	Animation anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		anim [animName].speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
