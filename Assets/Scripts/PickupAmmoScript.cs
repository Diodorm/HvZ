using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        print("collision detected");
        if (other.tag == "Player")
        {
            print("collsion is player");
			EventManager.TriggerEvent<CollectItemEvent, Vector3>(other.transform.position);
            Destroy(this.gameObject);
        }
    }
}
