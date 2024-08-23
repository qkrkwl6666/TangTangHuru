using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject chatPanel;
    public TextMeshProUGUI chatText;
    public Button chatButton;
    public Image npcImage;

    private float waitingChatDuration = 1f;
    private bool buttonClicked = false;

    private List<string> tutorialString;

    public TutorialGameInit tutorialGameInit;
    public TimeControl timeControl;

    public RectTransform levelUpPanel;

    private float defaultlevelupRect = 200;

    private float topLevelUpRectTop = 100f;
    private float topLevelUpRectBottom = 300f;

    public Transform tutorialContent;
    private Transform prevTutorialContent;
    public Transform radarBar;
    public GameObject treasure;
    public GameObject guardian;
    public GameObject player;

    private void OnDisable()
    {
        buttonClicked = false;
    }

    private void Awake()
    {
        chatButton.onClick.AddListener(OnChatButton);

        tutorialString = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).GetTutorialTexts();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        StartCoroutine(Tutorial2Start());
        StartCoroutine(WaitSec());
    }

    private void Update()
    {

    }

    public IEnumerator WaitSec()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        timeControl.StopTime();
    }

    public IEnumerator Tutorial2Start()
    {
        yield return StartCoroutine(ChatActive(1, 1, true));

        //timeControl.NormalTime();
    }

    public IEnumerator Tutorial3Start()
    {
        yield return StartCoroutine(ChatActive(2, 1, true));

        npcImage.gameObject.SetActive(false);
        // 위로 올려야함
        levelUpPanel.offsetMax = new Vector2(levelUpPanel.offsetMax.x, -topLevelUpRectTop);
        levelUpPanel.offsetMin = new Vector2(levelUpPanel.offsetMin.x, topLevelUpRectBottom);

        yield return StartCoroutine(ChatActive(3, 1, true));

        // 복원
        levelUpPanel.offsetMax = new Vector2(levelUpPanel.offsetMax.x, -defaultlevelupRect);
        levelUpPanel.offsetMin = new Vector2(levelUpPanel.offsetMin.x, defaultlevelupRect);

        // 버튼 선택 까지 대기
        while (true)
        {
            if (!levelUpPanel.parent.gameObject.activeSelf) break;

            yield return null;
        }

        timeControl.NormalTime();

        yield return new WaitForSeconds(1f);

        treasure.SetActive(true);
        // 탐지 게이지 강조
        radarBar.SetAsLastSibling();

        yield return StartCoroutine(ChatActive(5, 1, true));

        radarBar.SetAsFirstSibling();

        timeControl.NormalTime();
    }

    public IEnumerator Tutorial4Start()
    {
        yield return StartCoroutine(ChatActive(6, 2, true));

        timeControl.NormalTime();
    }

    public IEnumerator Tutorial5Start()
    {
        guardian.SetActive(true);

        yield return StartCoroutine(ChatActive(8, 1, true));

        timeControl.NormalTime();

        player.GetComponent<PlayerController>().StartStun(100f);
    }

    public void OnChatButton()
    {
        buttonClicked = true;
    }

    public IEnumerator ChatActive(int startIndex, int chatCount, bool npcActive = false)
    {
        timeControl.StopTime();

        int currentCount = startIndex;

        tutorialPanel.SetActive(true);
        chatPanel.SetActive(true);
        npcImage.gameObject.SetActive(npcActive);

        while (currentCount < chatCount + startIndex)
        {
            buttonClicked = false;
            chatText.text = tutorialString[currentCount];

            yield return new WaitForSecondsRealtime(waitingChatDuration);

            yield return new WaitUntil(() => buttonClicked);

            currentCount++;
        }

        timeControl.NormalTime();
        tutorialPanel.SetActive(false);
    }
}
