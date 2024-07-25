using UnityEngine;
using UnityEngine.UI;

public class ImageFadeInOut : MonoBehaviour
{
    private Image image;

    public float maxAlpha = 1.0f;
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    float timer = 0f;

    bool fadingIn = true;
    bool fadingOut = false;

    private void Start()
    {
        image = GetComponent<Image>();
        timer = 0f;
    }

    private void Update()
    {
        if (fadingIn)
        {
            FadeIn();
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (fadingOut)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(0f, maxAlpha, timer / fadeInDuration);
        Color color = image.color;
        color.a = alpha;
        image.color = color;

        if (timer >= fadeInDuration)
        {
            timer = 0f;
            fadingIn = false;
            fadingOut = true;
            color.a = maxAlpha;
            image.color = color;
        }
    }

    private void FadeOut()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(maxAlpha, 0f, timer / fadeOutDuration);
        Color color = image.color;
        color.a = alpha;
        image.color = color;

        if (timer >= fadeOutDuration)
        {
            timer = 0f;
            fadingOut = false;
            fadingIn = true;
            color.a = 0f;
            image.color = color;
        }
    }


}
