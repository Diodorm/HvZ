using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 5f;
    public Transform[] spawnPoints;
	public int maxSpawns = 5;
    public GameObject SpawnedZombies;

	private int spawnCount = 0;
    private GameObject spawnedObject;

    void Start ()
    {
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }
			
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		if (spawnCount >= maxSpawns) {
			CancelInvoke ("Spawn");
		} else {
			spawnedObject = Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
            spawnedObject.transform.parent = SpawnedZombies.transform;
		}
		spawnCount++;
    }
}
