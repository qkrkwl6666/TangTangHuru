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
    private int requiredStopCount = 10;  // 모바일 10 / pc 5

    private Vector2 scrollViewCenter = Vector2.zero;

    public float centeringDuration = 0.15f; // 중앙 정렬 애니메이션 시간
    public RectTransform contentRect; // 스크롤 뷰의 content RectTransform
    private List<RectTransform> stageRects = new ();

    private float magnitude = int.MaxValue;

    // 스테이지 UI 

    private int currentStage = 1; // Todo : 나중에 세이브된 스테이지 awake때 로드 1로 시작하자
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    private bool isFirstEnable = false;

    private Dictionary<string, StageData> stageTable;

    private Coroutine scrollCoroutine;

    public MainUI mainUI;

    public Button selectButton;

    private void Awake()
    {
        if (!DataTableManager.Instance.isTableLoad)
            DataTableManager.Instance.OnAllTableLoaded += InitStageUI;
        else
        {
            Debug.Log("세이브 이미 로드 완료됨");
            InitStageUI();
        }
        
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        scrollCoroutine = StartCoroutine(ScrollCheck());
        
        if (isFirstEnable == false)
        {
            isFirstEnable = true;
        }
        else
        {
            CenterOnStage(stageRects[GameManager.Instance.CurrentStage - 1]);
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

        if(currentStage <= SaveManager.SaveDataV1.MaxStage)
        {
            GameManager.Instance.CurrentStage = currentStage;
        }

        mainUI.SaveLoadMainStageText();
    }

    private void Start()
    {
        // 처음에만 코루틴 실행해서 다시 enable 할때 는 작동이 안됨
        scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);

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

    private void Update()
    {
        
    }

    private IEnumerator ScrollCheck()
    {
        yield return new WaitForSeconds(0.1f);

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
            // 스테이지의 월드 위치를 뷰포트의 로컬 위치로 변환
            Vector3 stageViewportPos = scrollRect.viewport.InverseTransformPoint(stageRects[i].TransformPoint(Vector3.zero));
            float distance = Mathf.Abs(stageViewportPos.x - viewportCenter);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestStage = stageRects[i];
                currentStage = i + 1;

                selectButton.interactable = i + 1 > SaveManager.SaveDataV1.MaxStage ? false : true;
            }
        }

        if (nearestStage != null)
        {
            CenterOnStage(nearestStage);

            titleText.text = stageTable[(currentStage).ToString()].Title;
            descText.text = stageTable[(currentStage).ToString()].Desc;
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

    private IEnumerator CoCenterOnStageNoDotTween(RectTransform stageRect)
    {
        yield return new WaitForSeconds(0.1f);

        float stageLeftEdge = stageRect.anchoredPosition.x - stageRect.rect.width * 0.5f;

        float contentWidth = contentRect.rect.width;

        float viewportWidth = scrollRect.viewport.rect.width;

        float targetNormalizedPos = (stageLeftEdge + stageRect.rect.width * 0.5f - viewportWidth * 0.5f) / (contentWidth - viewportWidth);

        targetNormalizedPos = Mathf.Clamp01(targetNormalizedPos);

        scrollRect.horizontalNormalizedPosition = targetNormalizedPos;

        gameObject.SetActive(false);
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

                if(name + 1 > SaveManager.SaveDataV1.MaxStage)
                {
                    rect.GetChild(1).gameObject.SetActive(true);
                }

                if (stageTable[(name + 1).ToString()].Texture != "-1")
                {
                    Addressables.LoadAssetAsync<Sprite>(stageTable[(name + 1).ToString()].Texture).Completed += (sprite) => 
                    {
                        rect.GetChild(0).GetComponent<Image>().sprite = sprite.Result;
                    };
                }

                if (name == stageTable.Count - 1)
                {
                    StartCoroutine(CoCenterOnStageNoDotTween(stageRects[GameManager.Instance.CurrentStage - 1]));
                    currentStage = GameManager.Instance.CurrentStage;
                }
            };
        }

    }

}
