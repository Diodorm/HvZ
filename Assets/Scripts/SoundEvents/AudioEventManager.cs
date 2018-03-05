using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{


	public AudioClip bushSound;
	public AudioClip collectItem;
	public AudioClip playerLands;
	public AudioClip timeAlert;
	public AudioClip goalSound;
	public AudioClip noAccessSound;


	private UnityAction<Vector3> bushSoundEventListener;
	private UnityAction<Vector3> collectItemEventListener;
	private UnityAction<Vector3> playerLandsEventListener;
	private UnityAction<Vector3> timeAlertEventListener;
	private UnityAction<Vector3> goalSoundEventListener;
	private UnityAction<Vector3> noAccessSoundEventListener;


	void Awake()
	{

		bushSoundEventListener = new UnityAction<Vector3>(bushSoundEventHandler);
		collectItemEventListener = new UnityAction<Vector3>(collectItemEventHandler);
		playerLandsEventListener = new UnityAction<Vector3>(playerLandsEventHandler);
		timeAlertEventListener = new UnityAction<Vector3>(timeAlertEventHandler);
		goalSoundEventListener = new UnityAction<Vector3>(goalSoundEventHandler);
		noAccessSoundEventListener = new UnityAction<Vector3>(noAccessSoundEventHandler);
	}


	// Use this for initialization
	void Start()
	{
	}


	void OnEnable()
	{

		EventManager.StartListening<BushSoundEvent, Vector3>(bushSoundEventListener);
		EventManager.StartListening<CollectItemEvent, Vector3>(collectItemEventListener);
		EventManager.StartListening<PlayerLandsEvent, Vector3>(playerLandsEventListener);
		EventManager.StartListening<TimeAlertEvent, Vector3>(timeAlertEventListener);
		EventManager.StartListening<GoalSoundEvent, Vector3>(goalSoundEventListener);
		EventManager.StartListening<NoAccessSoundEvent, Vector3>(noAccessSoundEventListener);

	}

	void OnDisable()
	{

		EventManager.StopListening<BushSoundEvent, Vector3>(bushSoundEventListener);
		EventManager.StopListening<CollectItemEvent, Vector3>(collectItemEventListener);
		EventManager.StopListening<PlayerLandsEvent, Vector3>(playerLandsEventListener);
		EventManager.StopListening<TimeAlertEvent, Vector3>(timeAlertEventListener);
		EventManager.StopListening<GoalSoundEvent, Vector3>(goalSoundEventListener);
		EventManager.StopListening<NoAccessSoundEvent, Vector3>(noAccessSoundEventListener);
	}



	// Update is called once per frame
	void Update()
	{
	}



	void bushSoundEventHandler(Vector3 worldPos)
	{
		AudioSource.PlayClipAtPoint(this.bushSound, worldPos);
	}

	void collectItemEventHandler(Vector3 worldPos)
	{
		AudioSource.PlayClipAtPoint(this.collectItem, worldPos);
	}

	void playerLandsEventHandler(Vector3 worldPos)
	{
		AudioSource.PlayClipAtPoint(this.playerLands, worldPos);
	}

	void timeAlertEventHandler(Vector3 worldPos)
	{
		AudioSource.PlayClipAtPoint(this.timeAlert, worldPos);
	}

	void goalSoundEventHandler(Vector3 worldPos)
	{
		AudioSource.PlayClipAtPoint(this.goalSound, worldPos);
	}

	void noAccessSoundEventHandler(Vector3 worldPos)
	{
		AudioSource.PlayClipAtPoint(this.noAccessSound, worldPos);
	}
}

