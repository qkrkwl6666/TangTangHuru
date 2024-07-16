using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class WeaponFadeInOut : MonoBehaviour
{
    private IAimer currAimer;
    private SpriteRenderer spriteRenderer;

    public float maxAlpha = 0.7f;
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    float timer = 0f;

    bool fadingIn = true;
    bool fadingOut = false;

    void Start()
    {
        currAimer = GetComponent<IAimer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    private void OnDisable()
    {
        timer = 0f;
        fadingIn = true;
        fadingOut = false;
    }

    private void Update()
    {
        if (fadingIn)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, maxAlpha, timer / fadeInDuration);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            if (timer >= fadeInDuration)
            {
                fadingIn = false;
                color.a = maxAlpha;
                spriteRenderer.color = color;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }


        if (timer > currAimer.LifeTime - fadeOutDuration)
        {
            fadingOut = true;
            timer = 0f;
        }

        if (fadingOut)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0f, timer / fadeOutDuration);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            if (timer >= fadeOutDuration)
            {
                fadingOut = false;
                color.a = 0f;
                spriteRenderer.color = color;
            }
        }
    }

}
