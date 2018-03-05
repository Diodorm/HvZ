using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;

    public int aggroRange;
    public int sneakingVisionReduction;
    public int zombieChaseGiveUpDistanceAddition;
    public int minimumDetectionRadius;

    private bool hasSeen;
    private int defaultAggroRange;
	private GameObject[] waypoints;
	private GameObject currentWaypoint;
    void Awake ()
    {
        //nav.enabled = true;
        hasSeen = false;
        defaultAggroRange = aggroRange;
        minimumDetectionRadius = 1;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		nav.enabled = true;
		waypoints = GameObject.FindGameObjectsWithTag ("waypoint");
        /*
        for (int i = 0; i < waypoints.Length; i ++)
        {
            Debug.Log(waypoints[i]);
        }
        */
            
        currentWaypoint = waypoints [Random.Range (0, waypoints.Length - 1)];
    }


    void Update ()
    {
        //crouching reduces zombie vision
        if(player.GetComponent<Animator>().GetBool("crouching") == true)
        {
            aggroRange = defaultAggroRange - sneakingVisionReduction;
        } else
        {
            aggroRange = defaultAggroRange;
        }
        //zombies have a minimum detection radius
        if(aggroRange <= 0)
        {
            aggroRange = minimumDetectionRadius;
        }

        //Debug.Log(player.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Bush"));
        //Debug.Log(player.GetComponent<Collider>().gameObject.layer);
        //Debug.Log(LayerMask.NameToLayer("Bush"));
        if(player.GetComponent<SimpleControlScript>().hidden == true)
        {
            print("zombie player is hidden");
            aggroRange = minimumDetectionRadius;
        }
        //Debug.Log(currentWaypoint);
        //Debug.Log(Vector3.Magnitude(player.transform.position - transform.position));
        if (((enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && Vector3.Magnitude(player.transform.position - transform.position) < aggroRange) || hasSeen) && enemyHealth.currentHealth > 0 )
        {
            nav.SetDestination (player.position);
            hasSeen = true;
            if(Vector3.Magnitude(player.transform.position - transform.position) > defaultAggroRange + zombieChaseGiveUpDistanceAddition)
            {
                hasSeen = false;
            } 
        }
        else
        {
			if (enemyHealth.currentHealth > 0) {
				nav.SetDestination (currentWaypoint.transform.position);
			}
			
            if (Vector3.Magnitude(currentWaypoint.transform.position - transform.position) < 1)
            {
                currentWaypoint = waypoints[Random.Range(0, waypoints.Length - 1)];
            }

        }
    }

}


