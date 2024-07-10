using System.Collections;
using System.Collections.Generic;
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
        FadeInOutRoutine();
    }

    private void Update()
    {

    }

    private void FadeInOutRoutine()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
        }

        timer = 0f;
        while (timer < visibleDuration)
        {
            timer += Time.deltaTime;
        }

        // ���̵� �ƿ�
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            tmp.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
        }

        timer = 0f;
        gameObject.SetActive(false);
    }


}
