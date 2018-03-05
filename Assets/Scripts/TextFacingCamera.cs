using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFacingCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        transform.LookAt(2 * transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}
