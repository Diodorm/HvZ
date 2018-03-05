using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public Text healthText;
	//public Text killCountText;
	//public int killCount;

    Animator anim;
    AudioSource playerAudio;
    //PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    //bool isDead = false;
    bool damaged;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        //playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
		//killCount = 0;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;

        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
		healthText.text = "HEALTH: " + currentHealth.ToString () + "%";
		if (currentHealth <= 50 && currentHealth >10) {
			healthText.color = new Color(244f/255.0f, 185f/255.0f, 66f/255.0f);
		} else if (currentHealth <= 10) {
			healthText.color = Color.red;
		} else {
			healthText.color = Color.white;
		}
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        //healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        //playerShooting.DisableEffects ();

        //anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        //playerShooting.enabled = false;

		//timeOfDeath = Time.timeSinceLevelLoad;

		anim.enabled = false;
		//StartCoroutine ("waitToRestart");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "health")
        {
            currentHealth = currentHealth + other.GetComponent<PickupHealthScript>().healthValue;
        }

    }


    //	IEnumerator waitToRestart()
    //	{
    //		yield return new WaitForSeconds (deathLength);
    //		Debug.Log("You died. Wait 5 secs to restart scene");
    //		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //	}
}
