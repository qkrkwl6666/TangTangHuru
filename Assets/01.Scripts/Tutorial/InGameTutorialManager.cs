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
        StartCoroutine(Tutorial2Start());
    }

    private void Update()
    {

    }

    public IEnumerator Tutorial2Start()
    {
        yield return StartCoroutine(ChatActive(1, 1, true));

        timeControl.NormalTime();
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

        tutorialPanel.SetActive(false);
    }
}
