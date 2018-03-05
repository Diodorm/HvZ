using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpeedBoostScript : MonoBehaviour {
    public int speedBoostModifier;
    public int speedBoostDuration;
	// Use this for initialization
	void Start () {
        if (speedBoostModifier <= 1)
        {
            speedBoostModifier = 2;
        }
        if (speedBoostDuration <= 0)
        {
            speedBoostDuration = 0;
        } else if (speedBoostDuration <= 3)
        {
            speedBoostDuration = 3;
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
