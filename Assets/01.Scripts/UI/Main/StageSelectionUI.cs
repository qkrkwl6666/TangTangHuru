using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class StageSelectionUI : MonoBehaviour
{
    public ScrollRect scrollRect;
    private bool isScrolling = false;
    private float stopThreshold = 100f;
    private int consecutiveStopCount = 0;
    private int requiredStopCount = 10;  // ����� 10 / pc 5

    private Vector2 scrollViewCenter = Vector2.zero;

    public float centeringDuration = 0.15f; // �߾� ���� �ִϸ��̼� �ð�
    public RectTransform contentRect; // ��ũ�� ���� content RectTransform
    public List<RectTransform> stageRects; // �� ���������� RectTransform ����Ʈ

    private float magnitude = int.MaxValue;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);
        StartCoroutine(ScrollCheck());

        scrollViewCenter = new Vector2(scrollRect.viewport.rect.width * 0.5f, scrollRect.viewport.rect.height * 0.5f);
    }

    private void OnScrollRectValueChange(Vector2 pos)
    {
        isScrolling = true;

        if (magnitude >= stopThreshold)
        {
            consecutiveStopCount = 0; // ��ũ���� ����� ������ ī��Ʈ ����
        }
    }

    private IEnumerator ScrollCheck()
    {
        while (true)
        {
            if (isScrolling)
            {
                var velocity = scrollRect.velocity;

                magnitude = velocity.magnitude;

                if (magnitude <= stopThreshold)
                {
                    consecutiveStopCount++;
                    if (consecutiveStopCount >= requiredStopCount && Touch.activeTouches.Count == 0)
                    {
                        isScrolling = false;
                        Debug.Log("����");
                        consecutiveStopCount = 0;
                        FindNearestStage();
                    }
                }
                else
                {
                    consecutiveStopCount = 0;
                }
            }

            yield return null;
        }
    }

    private void FindNearestStage()
    {
        float viewportCenter = scrollRect.viewport.rect.width * 0.5f;
        RectTransform nearestStage = null;
        float nearestDistance = float.MaxValue;

        foreach (var stageRect in stageRects)
        {
            // ���������� ���� ��ġ�� ����Ʈ�� ���� ��ġ�� ��ȯ
            Vector3 stageViewportPos = scrollRect.viewport.InverseTransformPoint(stageRect.TransformPoint(Vector3.zero));
            float distance = Mathf.Abs(stageViewportPos.x - viewportCenter);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestStage = stageRect;
            }
        }

        if (nearestStage != null)
        {
            Debug.Log("���� ����� ��������: " + nearestStage.name);
            CenterOnStage(nearestStage);
        }
    }

    private void CenterOnStage(RectTransform stageRect)
    {
        // ���������� ���� �����ڸ� ��ġ ���
        float stageLeftEdge = stageRect.anchoredPosition.x - stageRect.rect.width * 0.5f;

        // Content�� ��ü �ʺ�
        float contentWidth = contentRect.rect.width;

        // ScrollRect�� ����Ʈ �ʺ�
        float viewportWidth = scrollRect.viewport.rect.width;

        // ���������� �߾ӿ� ��ġ��Ű�� ���� ����ȭ�� ��ġ ���
        float targetNormalizedPos = (stageLeftEdge + stageRect.rect.width * 0.5f - viewportWidth * 0.5f) / (contentWidth - viewportWidth);

        // ����ȭ�� ��ġ�� 0~1 ������ ����� �ʵ��� ����
        targetNormalizedPos = Mathf.Clamp01(targetNormalizedPos);

        // DOTween�� ����Ͽ� �ε巴�� ��ũ��
        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
                   x => scrollRect.horizontalNormalizedPosition = x,
                   targetNormalizedPos, centeringDuration)
               .SetEase(Ease.OutQuad);
    }
}
