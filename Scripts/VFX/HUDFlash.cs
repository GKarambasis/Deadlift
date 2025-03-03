using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDFlash : MonoBehaviour
{
    public float fadeDuration;
    public float holdTime;

    Image hudImage;

    private float elapsedTime;
    private bool fadeIn;
    
    // Start is called before the first frame update
    void Start()
    {
        hudImage = GetComponent<Image>();

        SetAlpha(0);
    }

    public void FadeIn()
    {
        fadeIn = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetAlpha(alpha);

            if(elapsedTime >= fadeDuration)
            {
                fadeIn = false;
                elapsedTime = 0;
                //Invoke(nameof(FadeOut), holdTime);
                StartCoroutine(FadeOut());
            }
        }
    }

    IEnumerator FadeOut()
    {
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            SetAlpha(alpha);
            yield return null;
        }

        // Reset values after fading out
        elapsedTime = 0f;
    }



    private void SetAlpha(float alpha)
    {
        if (hudImage != null)
        {
            Color color = hudImage.color;
            color.a = alpha;
            hudImage.color = color;
        }
    }
}
