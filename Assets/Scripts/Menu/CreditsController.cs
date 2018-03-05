// Team Unity Lunacy
// Leo Chen, Daniel Kane, Rayner Kristanto, Frank Marzen, Lixin Wang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {

    private GameObject fadeToBlackImage;
    private GameObject[] creditSlides;

    private float currentTime = 0.0f;
    private static float fadeTime = 3.0f;
    private float timeInbetweenFades = fadeTime * 2 + 5f;

    private int curSlideIndex = 0;
    private bool transitionSlide = false;
    private bool slideAlreadyTransitioned = false;

    private float fadeToBlackImageAlpha;

	// Use this for initialization
	void Start () {

        fadeToBlackImage = GameObject.FindGameObjectWithTag("Fade To Black");
        creditSlides = GameObject.FindGameObjectsWithTag("Credit Slide");

        fadeToBlackImage.GetComponent<MeshRenderer>().material.color = Color.black;

        Color tempColor = fadeToBlackImage.GetComponent<MeshRenderer>().material.color;
        tempColor.a = 0.0f;
        fadeToBlackImage.GetComponent<MeshRenderer>().material.color = tempColor;

        for (int i = 0; i < creditSlides.Length; i++) {
            creditSlides[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        creditSlides[curSlideIndex].SetActive(true);

        if (Time.timeSinceLevelLoad - currentTime > timeInbetweenFades) {
            currentTime = Time.timeSinceLevelLoad;
            transitionSlide = true;
        }

        if (transitionSlide) {
            float prevFadeVal = fadeToBlackImageAlpha;

            fadeToBlackImageAlpha = Mathf.Lerp(0.0f, 1.0f, Mathf.PingPong ((Time.timeSinceLevelLoad - currentTime) / fadeTime, 1));

            float fadeDif = fadeToBlackImageAlpha - prevFadeVal;

            Color tempColor = fadeToBlackImage.GetComponent<MeshRenderer>().material.color;
            tempColor.a = fadeToBlackImageAlpha;
            fadeToBlackImage.GetComponent<MeshRenderer>().material.color = tempColor;

            if (Time.timeSinceLevelLoad - currentTime > fadeTime * 2) {
                transitionSlide = false;
            }

            if (fadeDif > 0.0f) {
                slideAlreadyTransitioned = true;
            }
            
            if (fadeDif < 0.0f && slideAlreadyTransitioned) {
                slideAlreadyTransitioned = false;

                creditSlides[curSlideIndex].SetActive(false);

                curSlideIndex++;

                if (curSlideIndex >= creditSlides.Length) {
                    // exit credits
                    SceneManager.LoadScene("menu");
                }
            }
        }
	}
}