using System.Collections;
using TMPro;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fadeInDuration = 0.5f; // 페이드 인
    public float fadeOutDuration = 0.5f; // 페이드 아웃
    public float visibleDuration = 0.5f; // 완전하게 보이는 상태 지속 시간

    private TextMeshPro tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeInOutRoutine());
    }

    private void Update()
    {

    }

    IEnumerator FadeInOutRoutine()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }

        yield return new WaitForSeconds(visibleDuration);

        // 페이드 아웃
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            yield return null;
        }

        gameObject.SetActive(false);
    }


}
