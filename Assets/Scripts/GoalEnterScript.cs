using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GoalEnterScript : MonoBehaviour {

	public GameOverManager GameOverManager;
	public bool reachedGoal;
	Animator anim;
	float restartTimer;

	public GameObject timer;
	public GameObject goalText;

	public Transform[] startLocations;
	public Transform[] goalLocations;
	public Transform platform;

	public AudioSource goalEnterSound;

	public GameObject[] ZombieManagers;
	public GameObject[] SpawnPoints;
	public GameObject[] ItemManagers;
	public GameObject[] ItemSpawnPoints;

	private int level = 0;

	void Awake()
	{
		anim = GameOverManager.GetComponent<Animator>();
		if (anim == null)
			Debug.Log ("Animator could not be found");
	}

	// Use this for initialization
	void Start ()
	{
		reachedGoal = false;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			EventManager.TriggerEvent<GoalSoundEvent, Vector3>(other.transform.position);
			print ("level:" + level);
			ZombieManagers [level].SetActive(false);
			SpawnPoints [level].SetActive(false);
			ItemManagers [level].SetActive(false);
			ItemSpawnPoints [level].SetActive (false);
			goalEnterSound.Play ();
			++level;

			if (level <= 2) {
				ZombieManagers [level].SetActive(true);
				SpawnPoints [level].SetActive(true);
				ItemManagers [level].SetActive(true);
				ItemSpawnPoints [level].SetActive (true);

				platform.SetPositionAndRotation(startLocations [level].position, startLocations [level].rotation);
				other.gameObject.transform.SetPositionAndRotation (startLocations [level].position, startLocations [level].rotation);
				transform.parent.SetPositionAndRotation(goalLocations [level].position, goalLocations [level].rotation);

				goalText.GetComponent<Text> ().text = "Goal: "+goalLocations [level].name;
				timer.GetComponent<CountdownClock> ().alertTime = 150.0f;
				timer.GetComponent<CountdownClock> ().RestartTimer (3,0);

			} else {
				
				anim.SetTrigger("GameOver");
				other.gameObject.SetActive (false);
				// .. increment a timer to count up to restarting.
				restartTimer += Time.deltaTime;

				// .. if it reaches the restart delay...
				if(restartTimer >= 2f)
				{
				// .. then game over
					SceneManager.LoadScene("menu");
				}
			}

		}
	}
}
