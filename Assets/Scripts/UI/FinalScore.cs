using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalScore : MonoBehaviour
{
	public static int score;


	Text text;


	void Awake ()
	{
		text = GetComponent <Text> ();
		score = ScoreManager.score;
	}


	void Update ()
	{
		text.text = "Zombies Killed: " + score.ToString();
	}
}