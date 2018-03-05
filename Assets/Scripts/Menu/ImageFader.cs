// Team Unity Lunacy
// Leo Chen, Daniel Kane, Rayner Kristanto, Frank Marzen, Lixin Wang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour {

	private RawImage[] images;
    private RawImage curImage;
    private float lerpedAlpha = 0.0f;
    private int imageIndex;

    private bool fade = true;

	private float currentTime = 0f;
	private static float fadeTime = 5f;
	private float waitingTime = fadeTime * 2 + 5f;

	void Start () {
        
		images = (RawImage[]) GetComponentsInChildren<RawImage> ();

        // setting all images' alpha values to 0.0
        for (int i = 0; i < images.Length; i++) {
            Color tempColor = images[i].color;
            tempColor.a = 0.0f ;
            images[i].color = tempColor;
        }

        imageIndex = Random.Range(0, images.Length);
        curImage = images[imageIndex];
	}
	

	void Update () {

		if (Time.timeSinceLevelLoad - currentTime > waitingTime) {
			fade = true;
			currentTime = Time.timeSinceLevelLoad;
//            Debug.Log (Time.timeSinceLevelLoad + " fading begins");
		}

		if (fade) {
			lerpedAlpha = Mathf.Lerp (0.0f, 1.0f, Mathf.PingPong ((Time.timeSinceLevelLoad - currentTime) / fadeTime, 1));

            Color tempColor = curImage.color;
            tempColor.a = lerpedAlpha;
            curImage.color = tempColor;

			if (Time.timeSinceLevelLoad - currentTime > fadeTime * 2) {
				fade = false;

                // change images if the current image's alpha value is below threshold
                if (curImage.color.a <= 0.005f) {
                    imageIndex = (imageIndex + 1) % images.Length;
                    curImage = images[imageIndex];
//                    print(curImage.name);
                }

//				Debug.Log (Time.timeSinceLevelLoad + " fading is done");
			}
		}

	}
}
