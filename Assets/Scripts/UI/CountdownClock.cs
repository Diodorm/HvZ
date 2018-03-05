using UnityEngine;
using UnityEngine.UI;

public class CountdownClock : MonoBehaviour
{
	public int  Minutes = 0;
	public int  Seconds = 0;
	public GameObject inGameMenu;
	public GameObject gameOver;
	public AudioSource timerAlert;
	public float alertTime = 90.0f;

	private Text    m_text;
	private float   m_leftTime;
	private Animator gameOverAnim;

	private void Awake()
	{
		m_text = GetComponent<Text>();
		m_leftTime = GetInitialTime();
		gameOverAnim = gameOver.GetComponent<Animator>();
		timerAlert.PlayDelayed (alertTime);
	}

	public void RestartTimer(int minutes, int seconds) {
		print ("restart time");
		Minutes = minutes;
		Seconds = seconds;
		m_leftTime = GetInitialTime();
		timerAlert.PlayDelayed (150.0f);
	}

	private void Update()
	{
		if (m_leftTime > 0f && !inGameMenu.activeSelf)
		{
			//  Update countdown clock
			m_leftTime -= Time.deltaTime;
			Minutes = GetLeftMinutes();
			Seconds = GetLeftSeconds();

			//  Show current clock
			if (m_leftTime > 0f)
			{
				m_text.text = "Time : " + Minutes + ":" + Seconds.ToString("00");
			}
			else
			{
				//  The countdown clock has finished
				m_text.text = "Time : 0:00";
				gameOverAnim.SetTrigger("GameOver");
			}
			if (Minutes == 0 && Seconds <= 30) {
				m_text.color = Color.red;
			} else {
				m_text.color = Color.yellow;
			}
		}
	}

	private float GetInitialTime()
	{
		return Minutes * 60f + Seconds;
	}

	private int GetLeftMinutes()
	{
		return Mathf.FloorToInt(m_leftTime / 60f);
	}

	private int GetLeftSeconds()
	{
		return Mathf.FloorToInt(m_leftTime % 60f);
	}

}