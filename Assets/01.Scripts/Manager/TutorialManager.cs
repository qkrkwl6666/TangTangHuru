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

    public GameObject emptyGameObject;

    #region UI
    public Button tutorialButton;

    // ����
    public Button appraiseButton;
    public GameObject appraiseContent;
    public GameObject appraisePanel;
    // ���� ��ư
    public Button onAppraiseButton;

    // ��� Ȯ�� ��ư
    public Button itemInfoComfirmButton;

    // �κ��丮 ��ư
    public Button inventoryButton;

    // �κ��丮 content
    public Transform inventoryContent;

    // ��� �˾�
    public GameObject upgradeGo; // ���׷��̵� ��ư
    public Button tierUpPopUpButton; // �±� �˾� ��ư

    // �±� �˾�
    public Transform tierUpContent;

    // �±� ��ư
    public Button tierUpButton;

    // ���� ���� ��ư
    public Button weaponEquipButton;

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

        // ȭ�� ��Ŀ�� �� ��ư ü����
        currentbuttonContent = tutorialButton.transform.parent;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);

        tutorialButton.transform.SetParent(tutorialButtonContent, true);
    }

    public IEnumerator ChatActive(int startIndex, int chatCount, bool npcActive = false)
    {
        int currentCount = startIndex;

        buttonClicked = false;

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
        if (SaveManager.SaveDataV1.isTutorialCompleted) return;

        tutorialString = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).GetTutorialTexts();


        if (!SaveManager.isSaveFile && !GameManager.Instance.isTutorialSceneEnd)
        {
            StartCoroutine(Tutorial1Start());
        }
        else if (SaveManager.isSaveFile && !GameManager.Instance.isTutorialSceneEnd && !SaveManager.SaveDataV1.isTutorialCompleted)
        {
            StartCoroutine(Tutorial1Start());
        }

        // Ʃ�丮�� ���� ������ ���
        if (GameManager.Instance.isTutorialSceneEnd)
        {
            GameManager.Instance.isTutorialSceneEnd = false;
            StartCoroutine(TutorialSceneEndStart());
        }
    }

    public IEnumerator TutorialSceneEndStart()
    {
        yield return StartCoroutine(ChatActive(9, 1, true));

        // ���� ��ư ����
        // ȭ�� ��Ŀ�� �� ��ư ü����

        buttonClicked = false;

        currentbuttonContent = appraisePanel.transform.parent;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);

        appraisePanel.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        appraisePanel.transform.SetParent(currentbuttonContent, true);

        yield return new WaitForSeconds(0.1f);

        // ���� UI �̵�
        //// ���� �ھ� ���� ����
        tutorialPanel.SetActive(true);
        buttonClicked = false;

        var core = appraiseContent.transform.GetChild(0).gameObject;

        core.GetComponent<Button>().onClick.AddListener(OnChatButton);

        core.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        core.transform.SetParent(appraiseContent.transform, false);
        core.transform.SetSiblingIndex(0);

        // ���� ��ư ���� �� ���� ���
        tutorialPanel.SetActive(true);
        buttonClicked = false;
        onAppraiseButton.onClick.AddListener(OnChatButton);

        currentbuttonContent = onAppraiseButton.transform.parent;
        onAppraiseButton.transform.SetParent(tutorialButtonContent, true);
        yield return new WaitUntil(() => buttonClicked);
        tutorialPanel.SetActive(false);

        onAppraiseButton.transform.SetParent(currentbuttonContent, true);

        buttonClicked = false;

        // chat
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(ChatActive(10, 1, true));

        // ��� Ȯ�� ��ư
        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);
        tutorialPanel.SetActive(true);

        buttonClicked = false;
        itemInfoComfirmButton.onClick.AddListener(OnChatButton);

        currentbuttonContent = itemInfoComfirmButton.transform.parent;
        itemInfoComfirmButton.transform.SetParent(tutorialButtonContent, true);
        yield return new WaitUntil(() => buttonClicked);

        itemInfoComfirmButton.transform.SetParent(currentbuttonContent, true);
        buttonClicked = false;

        // �κ��丮 �̵�
        inventoryButton.onClick.AddListener(OnChatButton);

        currentbuttonContent = inventoryButton.transform.parent;
        inventoryButton.transform.SetParent(tutorialButtonContent, true);
        yield return new WaitUntil(() => buttonClicked);

        buttonClicked = false;

        inventoryButton.transform.SetParent(currentbuttonContent, true);

        // ��� ǥ��
        yield return new WaitForSeconds(0.1f);

        var item = inventoryContent.GetChild(0);
        item.GetComponentInChildren<Button>().onClick.AddListener(OnChatButton);

        item.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        buttonClicked = false;

        item.SetParent(inventoryContent, true);
        item.transform.SetSiblingIndex(0);

        yield return new WaitForSeconds(0.3f);

        // ��� ��ȭ
        upgradeGo.GetComponentInChildren<Button>().onClick.AddListener(OnChatButton);

        currentbuttonContent = upgradeGo.transform.parent;
        upgradeGo.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        upgradeGo.transform.SetParent(currentbuttonContent, true);

        buttonClicked = false;

        // ��� ��ȭ �Ϸ� �� ä��

        yield return StartCoroutine(ChatActive(12, 1, true));

        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);
        tutorialPanel.SetActive(true);

        // ��� �±�

        buttonClicked = false;

        tierUpPopUpButton.onClick.AddListener(OnChatButton);

        currentbuttonContent = tierUpPopUpButton.transform.parent;
        tierUpPopUpButton.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        buttonClicked = false;

        tierUpPopUpButton.transform.SetParent(currentbuttonContent, true);

        yield return new WaitForSeconds(0.3f);

        // �±� â 

        var item2 = tierUpContent.GetChild(0);

        item2.GetComponentInChildren<Button>().onClick.AddListener(OnChatButton);
        currentbuttonContent = item2.transform.parent;
        item2.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        item2.SetParent(currentbuttonContent, true);
        item2.transform.SetSiblingIndex(0);

        buttonClicked = false;

        yield return StartCoroutine(ChatActive(13, 1, true));

        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);
        tutorialPanel.SetActive(true);

        buttonClicked = false;

        // �±� ��ư
        tierUpButton.onClick.AddListener(OnChatButton);

        currentbuttonContent = tierUpButton.transform.parent;
        tierUpButton.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        tierUpButton.transform.SetParent(currentbuttonContent, true);

        buttonClicked = false;

        yield return StartCoroutine(ChatActive(11, 1, true)); // ���ο� ���⸦ ���� �ϼ���

        chatPanel.SetActive(false);
        npcImage.gameObject.SetActive(false);
        tutorialPanel.SetActive(true);
        buttonClicked = false;

        // ���� ���� ����
        weaponEquipButton.onClick.AddListener(OnChatButton);

        currentbuttonContent = weaponEquipButton.transform.parent;
        weaponEquipButton.transform.SetParent(tutorialButtonContent, true);

        yield return new WaitUntil(() => buttonClicked);

        weaponEquipButton.transform.SetParent(currentbuttonContent, false);

        yield return StartCoroutine(ChatActive(14, 1, true));

        yield return new WaitForSeconds(1.0f);

        weaponEquipButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        SaveManager.SaveDataV1.isTutorialCompleted = true;

        GameManager.Instance.SaveGame();
    }

    public void OnTutorialStartButton()
    {
        GameManager.Instance.LoadSceneAsync(Defines.tutorialScene);
    }

}
