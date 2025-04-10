using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasgroup;
    public bool fadeIn = false;
    public bool fadeOut = false;

    public float timeToFade;

    private void Update()
    {
        if(fadeIn == true)
        {
            if(canvasgroup.alpha < 1)
            {
                canvasgroup.alpha += timeToFade * Time.deltaTime;
                if(canvasgroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if(fadeOut == true)
        {
            if(canvasgroup.alpha >= 0)
            {
                canvasgroup.alpha -= timeToFade * Time.deltaTime;
                if (canvasgroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
