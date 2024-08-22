using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject chatPanel;
    public TextMeshProUGUI chatText;
    public Button chatButton;
    public Image npcImage;

    private float waitingChatDuration = 1f;
    private bool buttonClicked = false;

    private List<string> tutorialString;

    private Transform currentbuttonContent;
    public Transform tutorialButtonContent;

    #region UI
    public Button tutorialButton;
    public Button appraiseButton;
    public Transform appraiseContent;

    #endregion

    private void OnDisable()
    {
        buttonClicked = false;
    }

    private void Awake()
    {
        chatButton.onClick.AddListener(OnChatButton);
        appraiseButton.onClick.AddListener(OnChatButton);
        tutorialButton.onClick.AddListener(OnTutorialStartButton);
    }

    private void Start()
    {
        if (!DataTableManager.Instance.isTableLoad)
            DataTableManager.Instance.OnAllTableLoaded += LoadTutorialString;
        else
        {
            LoadTutorialString();
        }
    }

    public IEnumerator Tutorial1Start()
    {
        yield return StartCoroutine(ChatActive(0, 1, true));

        tutorialButton.gameObject.SetActive(true);

        // 화면 포커싱 후 버튼 체인지
        currentbuttonContent = tutorialButton.transform.parent;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);

        tutorialButton.transform.SetParent(tutorialButtonContent, true);
    }

    public IEnumerator ChatActive(int startIndex, int chatCount, bool npcActive = false)
    {
        int currentCount = startIndex;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(true);
        npcImage.gameObject.SetActive(npcActive);

        while (currentCount < chatCount + startIndex)
        {
            buttonClicked = false;
            chatText.text = tutorialString[currentCount];

            yield return new WaitForSeconds(waitingChatDuration);

            yield return new WaitUntil(() => buttonClicked);

            currentCount++;
        }

        tutorialPanel.SetActive(false);
    }

    public void OnChatButton()
    {
        buttonClicked = true;
    }

    public void LoadTutorialString()
    {
        tutorialString = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).GetTutorialTexts();

        if (!SaveManager.isSaveFile)
        {
            StartCoroutine(Tutorial1Start());
        }

        // 튜토리얼 씬이 끝낫을 경우
        if(GameManager.Instance.isTutorialSceneEnd)
        {
            StartCoroutine(TutorialSceneEndStart());
        }
    }

    public IEnumerator TutorialSceneEndStart()
    {
        yield return StartCoroutine(ChatActive(9, 1, true));

        // 감정 버튼 강조
        // 화면 포커싱 후 버튼 체인지

        RectTransform appraiseRect = appraiseButton.GetComponent<RectTransform>();
        currentbuttonContent = appraiseRect.parent;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);

        appraiseRect.SetParent(tutorialButtonContent, true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(appraiseRect);

        yield return new WaitUntil(() => buttonClicked);
        appraiseRect.SetParent(currentbuttonContent, true);

        // 감정 UI 이동
        // 현재 코어 선택 강조
        tutorialPanel.SetActive(true);
        var core = appraiseContent.GetChild(0).gameObject;

        core.GetComponent<Button>().onClick.AddListener(OnChatButton);

        core.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        core.transform.SetParent(appraiseContent, true);


        yield return null;
    }

    public void OnTutorialStartButton()
    {
        GameManager.Instance.LoadSceneAsync(Defines.tutorialScene);
    }

}
