using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 1f;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;
    Transform player;
	float deathTimer;



    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        //hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball")
        {
            TakeDamage(100, other.transform.position);
        }
    }


    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "ball")
    //    {
    //        playerInRange = false;
    //    }
    //}

    void Update ()
    {	
		if (isDead) {
			deathTimer += Time.deltaTime;

			if (deathTimer >= 5f) {
				StartSinking ();
			}
		}
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
//        hitParticles.transform.position = hitPoint;
//        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }

    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;
		GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
		Debug.Log ("you killed a zombie");
		ScoreManager.score += 1;
		FinalScore.score += 1;

    }


    public void StartSinking ()
    {
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        Destroy (gameObject, 2f);
    }
}
