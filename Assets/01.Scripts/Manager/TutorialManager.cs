using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

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

    #region 버튼 모음 
    public Button tutorialButton;

    #endregion

    private void OnDisable()
    {
        buttonClicked = false;
    }

    private void Awake()
    {
        chatButton.onClick.AddListener(OnChatButton);
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
        int currentCount = 0;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(true);
        npcImage.gameObject.SetActive(npcActive);

        while (currentCount < chatCount)
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
    }

    public void OnTutorialStartButton()
    {
        GameManager.Instance.LoadSceneAsync(Defines.tutorialScene);
    }
}
