using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFadeInOut : MonoBehaviour
{

    public float maxAlpha = 1.0f;
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    float timer = 0f;

    bool fadingIn = true;
    bool fadingOut = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    private void Update()
    {
        if (fadingIn)
        {
            FadeIn();
        }

        if(!fadingIn && !fadingOut)
        {
            timer += Time.deltaTime;
            if( timer > 5f)
            {
                fadingOut = true;
                timer = 0f;
            }
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
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;

        if (timer >= fadeInDuration)
        {
            timer = 0f;
            fadingIn = false;
            fadingOut = false;
            color.a = maxAlpha;
            spriteRenderer.color = color;
        }
    }

    private void FadeOut()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(maxAlpha, 0f, timer / fadeOutDuration);
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;

        if (timer >= fadeOutDuration)
        {
            timer = 0f;
            fadingOut = false;
            fadingIn = true;
            color.a = 0f;
            spriteRenderer.color = color;
            gameObject.SetActive(false);
        }
    }
}
