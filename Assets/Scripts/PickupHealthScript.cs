using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealthScript : MonoBehaviour {
    public int healthValue;
	// Use this for initialization
	void Start () {
        if(healthValue <= 0)
        {
            healthValue = 0;
        } else if (healthValue <= 25) {
            healthValue = 25;
        }
		
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
