using UnityEngine;
using Color = UnityEngine.Color;

public class WeaponFadeInOut : MonoBehaviour
{
    private IAimer currAimer;
    private SpriteRenderer spriteRenderer;

    public float maxAlpha = 0.7f;
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    private float timer = 0f;

    void Start()
    {
        currAimer = GetComponentInParent<IAimer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    private void OnEnable()
    {
        ResetEffect();
    }

    private void Update()
    {
        if (timer <= fadeInDuration)
        {
            FadeIn();
        }
        else if (timer > currAimer.LifeTime - fadeOutDuration && timer < currAimer.LifeTime)
        {
            FadeOut();
        }

        timer += Time.deltaTime;
    }

    private void ResetEffect()
    {
        timer = 0f;
    }

    private void FadeIn()
    {
        float alpha = Mathf.Lerp(0f, maxAlpha, timer / fadeInDuration);
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void FadeOut()
    {
        float alpha = Mathf.Lerp(maxAlpha, 0f, (timer - (currAimer.LifeTime - fadeOutDuration)) / fadeOutDuration);
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

}
