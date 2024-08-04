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
    private int requiredStopCount = 10;  // 모바일 10 / pc 5

    private Vector2 scrollViewCenter = Vector2.zero;

    public float centeringDuration = 0.15f; // 중앙 정렬 애니메이션 시간
    public RectTransform contentRect; // 스크롤 뷰의 content RectTransform
    public List<RectTransform> stageRects; // 각 스테이지의 RectTransform 리스트

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
            consecutiveStopCount = 0; // 스크롤이 변경될 때마다 카운트 리셋
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
                        Debug.Log("멈춤");
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
            // 스테이지의 월드 위치를 뷰포트의 로컬 위치로 변환
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
            Debug.Log("가장 가까운 스테이지: " + nearestStage.name);
            CenterOnStage(nearestStage);
        }
    }

    private void CenterOnStage(RectTransform stageRect)
    {
        // 스테이지의 왼쪽 가장자리 위치 계산
        float stageLeftEdge = stageRect.anchoredPosition.x - stageRect.rect.width * 0.5f;

        // Content의 전체 너비
        float contentWidth = contentRect.rect.width;

        // ScrollRect의 뷰포트 너비
        float viewportWidth = scrollRect.viewport.rect.width;

        // 스테이지를 중앙에 위치시키기 위한 정규화된 위치 계산
        float targetNormalizedPos = (stageLeftEdge + stageRect.rect.width * 0.5f - viewportWidth * 0.5f) / (contentWidth - viewportWidth);

        // 정규화된 위치가 0~1 범위를 벗어나지 않도록 조정
        targetNormalizedPos = Mathf.Clamp01(targetNormalizedPos);

        // DOTween을 사용하여 부드럽게 스크롤
        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
                   x => scrollRect.horizontalNormalizedPosition = x,
                   targetNormalizedPos, centeringDuration)
               .SetEase(Ease.OutQuad);
    }
}
