using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TMPro;

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
    private List<RectTransform> stageRects = new ();

    private float magnitude = int.MaxValue;

    // �������� UI 

    private int currentStage = 0; // Todo : ���߿� ���̺�� �������� awake�� �ε� 1�� ��������
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    private bool isFirstEnable = false;

    private Dictionary<string, StageData> stageTable;

    private Coroutine scrollCoroutine;
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        scrollCoroutine = StartCoroutine(ScrollCheck());
        //Debug.Log(GameManager.Instance.CurrentStage);
        if (isFirstEnable == false)
        {
            InitStageUI();
            isFirstEnable = true;
        }
        else
        {
            // ���ӸŴ��� ���������� �߾� �ʱ�ȭ
            CenterOnStage(stageRects[GameManager.Instance.CurrentStage]);
        }
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();

        isScrolling = false;
        consecutiveStopCount = 0;

        if(scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
            scrollCoroutine = null;
        }
        //mainStageText.text = stageTable[currentStage.ToString()].Title;
        GameManager.Instance.CurrentStage = currentStage;

        Debug.Log(GameManager.Instance.CurrentStage);
    }

    private void Start()
    {
        // ó������ �ڷ�ƾ �����ؼ� �ٽ� enable �Ҷ� �� �۵��� �ȵ�
        scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);

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

    private void Update()
    {
        
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

        for(int i = 0; i < stageRects.Count; i++)
        {
            // ���������� ���� ��ġ�� ����Ʈ�� ���� ��ġ�� ��ȯ
            Vector3 stageViewportPos = scrollRect.viewport.InverseTransformPoint(stageRects[i].TransformPoint(Vector3.zero));
            float distance = Mathf.Abs(stageViewportPos.x - viewportCenter);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestStage = stageRects[i];
                currentStage = i;
            }
        }

        if (nearestStage != null)
        {
            CenterOnStage(nearestStage);

            titleText.text = stageTable[(currentStage + 1).ToString()].Title;
            descText.text = stageTable[(currentStage + 1).ToString()].Desc;
            
        }
    }

    private void CenterOnStage(RectTransform stageRect)
    {
        float stageLeftEdge = stageRect.anchoredPosition.x - stageRect.rect.width * 0.5f;

        float contentWidth = contentRect.rect.width;

        float viewportWidth = scrollRect.viewport.rect.width;

        float targetNormalizedPos = (stageLeftEdge + stageRect.rect.width * 0.5f - viewportWidth * 0.5f) / (contentWidth - viewportWidth);

        targetNormalizedPos = Mathf.Clamp01(targetNormalizedPos);

        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
                   x => scrollRect.horizontalNormalizedPosition = x,
                   targetNormalizedPos, centeringDuration)
               .SetEase(Ease.OutQuad);
    }

    public void InitStageUI()
    {
        stageTable = DataTableManager.Instance.Get<StageTable>(DataTableManager.stage).stageTable;

        Addressables.InstantiateAsync(Defines.emptyRect, contentRect).Completed += (x) =>
        {
            var go = x.Result;
            go.transform.SetAsFirstSibling();
        };

        Addressables.InstantiateAsync(Defines.emptyRect, contentRect).Completed += (x) =>
        {
            var go = x.Result;
            go.transform.SetAsLastSibling();
        };

        for (int i = 0; i < stageTable.Count; i++) 
        {
            int name = i;

            Addressables.InstantiateAsync(Defines.stageImage, contentRect).Completed 
                += (stage) =>
            { 
                var rect = stage.Result.GetComponent<RectTransform>();
                rect.gameObject.name = $"{name}";

                stageRects.Add(rect);

                if(name == stageTable.Count - 1)
                {
                    CenterOnStage(stageRects[GameManager.Instance.CurrentStage]);
                }
            };
        }


    }
}
