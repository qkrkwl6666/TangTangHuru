using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fadeInDuration = 0.5f; // 페이드 인
    public float fadeOutDuration = 0.5f; // 페이드 아웃
    public float visibleDuration = 0.5f; // 완전하게 보이는 상태 지속 시간

    private TextMeshPro tmp;

    IEnumerator fade;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        fade = FadeInOutRoutine();
    }

    void OnEnable()
    {
        StartCoroutine(fade);
    }

    private void OnDisable()
    {
        StopCoroutine(fade);
    }

    private IEnumerator FadeInOutRoutine()
    {
        // 페이드 인
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }

        // 잠깐 동안 완전히 보이는 상태 유지
        yield return new WaitForSeconds(visibleDuration);

        // 페이드 아웃
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            yield return null;
        }

        // 비활성화
        gameObject.SetActive(false);
    }


}
