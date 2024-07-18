using System.Collections;
using TMPro;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fadeInDuration = 0.5f; // ���̵� ��
    public float fadeOutDuration = 0.5f; // ���̵� �ƿ�
    public float visibleDuration = 0.5f; // �����ϰ� ���̴� ���� ���� �ð�

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

        // ���̵� �ƿ�
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
