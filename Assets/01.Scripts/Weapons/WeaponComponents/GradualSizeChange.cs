using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualSizeChange : MonoBehaviour
{
    private IAimer currAimer;

    private Vector3 baseScale = Vector3.zero;
    float timer = 0f;

    float size = 1f;

    bool growing = true;

    void Start()
    {
        currAimer = GetComponent<IAimer>();
        timer = 0f;
    }

    private void OnEnable()
    {
        timer = 0f;
        baseScale = gameObject.transform.localScale;
        growing = true;
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = baseScale;  // ��Ȱ��ȭ�� �� ũ�⸦ ������� ����
    }

    void Update()
    {
        if (!growing)
            return;

        if (timer < currAimer.LifeTime)
        {
            timer += Time.deltaTime;
            Grow();
        }
        else
        {
            growing = false;
            timer = 0f;
        }
    }

    private void Grow()
    {
        size = Mathf.Lerp(1, 3, timer / currAimer.LifeTime);
        gameObject.transform.localScale = baseScale * size;
    }
}
