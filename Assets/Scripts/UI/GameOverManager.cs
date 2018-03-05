using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 0.5f;
	public GoalEnterScript goalEnterScript;

    Animator anim;
	float restartTimer;
	float deathTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
			deathTimer += Time.deltaTime;

			if (deathTimer >= restartDelay) {
				
				anim.SetTrigger("GameOver");
//
//				// .. increment a timer to count up to restarting.
//				restartTimer += Time.deltaTime;
//
//				// .. if it reaches the restart delay...
//				if(restartTimer >= restartDelay)
//				{
//					// .. then game over
//					SceneManager.LoadScene("menu");
//				}
			}

        }

//		if (goalEnterScript.goalReached == true) {
//			anim.SetTrigger("GameOver");
//
//			// .. increment a timer to count up to restarting.
//			restartTimer += Time.deltaTime;
//
//			// .. if it reaches the restart delay...
//			if(restartTimer >= restartDelay)
//			{
//				// .. then game over
//				SceneManager.LoadScene("menu");
//			}
//
//		}

    }
}
