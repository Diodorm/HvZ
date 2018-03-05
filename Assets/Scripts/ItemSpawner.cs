using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RespawnItem
{
    public float timeSinceTaken; // the time since the player picked up the item
    public bool itemCurLoaded = false;
    public Transform spawnLocation;
}

public class ItemSpawner : MonoBehaviour {

    public GameObject[] spawnLocations;
    public GameObject item;
    public float respawnTime = 60.0f; // in seconds
    private RespawnItem[] spawnPoints;

	// Use this for initialization
	void Start () {
        //print("started");
        spawnPoints = new RespawnItem[spawnLocations.Length];
        //print("spawnPoints length:" + spawnPoints.Length);
        //print("spawnLocations length:" + spawnLocations.Length);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = new RespawnItem();
            spawnPoints[i].spawnLocation = spawnLocations[i].transform;
            spawnPoints[i].timeSinceTaken = 0.0f;
            spawnPoints[i].itemCurLoaded = true;
            GameObject newItem = Instantiate(item, new Vector3(0,0,0), spawnPoints[i].spawnLocation.rotation);
            newItem.transform.SetParent(spawnLocations[i].transform, false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            if (spawnLocations[i].transform.childCount <= 0 && spawnPoints[i].itemCurLoaded)
            {
                print("time reset");
                spawnPoints[i].timeSinceTaken = 0.0f;
                spawnPoints[i].itemCurLoaded = false;
            }
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!spawnPoints[i].itemCurLoaded && spawnPoints[i].timeSinceTaken > respawnTime)
            {

                GameObject newItem = Instantiate(item, new Vector3(0,0,0), spawnPoints[i].spawnLocation.rotation);
                newItem.transform.parent = spawnLocations[i].transform;
                spawnPoints[i].itemCurLoaded = true;

            }
            
            if (!spawnPoints[i].itemCurLoaded)
            {
                spawnPoints[i].timeSinceTaken = spawnPoints[i].timeSinceTaken + Time.deltaTime;
            }
        }
        //print(spawnPoints[0].timeSinceTaken + "  :  " + spawnPoints[1].timeSinceTaken + "  :  " + spawnPoints[2].timeSinceTaken + "  :  " + spawnPoints[3].timeSinceTaken + "  :  " + spawnPoints[4].timeSinceTaken);
	}
}
