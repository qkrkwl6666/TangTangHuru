using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour, IPlayerObserver
{
    public GameObject treasurePrefab;

    private Transform playerTransform;
    private PlayerSubject playerSubject;

    // ���� ���� ŉ�� UI
    private Slider treasureBar;
    // ���̴� UI
    public Slider radarBar;

    // ����ġ UI
    public Slider expBar;
    // ���� ü�� UI
    public Slider bossHpBar;
    // ���� UI
    public GameObject coinUI;
    // ���� Text
    public TextMeshProUGUI coinText;

    // Ÿ�̸� Text
    public TextMeshProUGUI stageTimer;

    // ���� Ŭ���� UI 
    public GameObject clearUI;

    // ���̽�ƽ UI
    public GameObject joystickUI;

    // ���� Ŭ���� UI
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI killText;

    // ���� UI
    public GameObject pauseUI;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        treasureBar = Instantiate(treasurePrefab, playerTransform).GetComponentInChildren<Slider>();
        treasureBar.gameObject.SetActive(false);
    }

    public void ActiveGameClearUI()
    {
        bossHpBar.gameObject.SetActive(false);
        radarBar.gameObject.SetActive(false);
        coinUI.gameObject.SetActive(false);
        stageTimer.gameObject.SetActive(false);
        joystickUI.gameObject.SetActive(false);

        clearUI.gameObject.SetActive(true);
    }

    public void SetGameClearUI(int gold, int kill)
    {
        goldText.text = $"ȹ���� ��� : {gold.ToString()}";
        killText.text = $"óġ�� ���� �� : {kill.ToString()}";
    }

    #region �Ͻ�����
    public void PauseButton()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }

    public void ContinueButton()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }

    public void ExitMainButton()
    {
        GameManager.Instance.LoadSceneAsync("InventoryScene");
    }

    #endregion

    public void SetActiveExpBar(bool active)
    {
        expBar.gameObject.SetActive(active);
    }

    public void SetActiveBossHpBar(bool active)
    {
        bossHpBar.gameObject.SetActive(active);
    }

    public void SetActiveTreasureBar(bool active)
    {
        treasureBar.gameObject.SetActive(active);
    }

    public void UpdateBossHpBar(float value)
    {
        bossHpBar.value = value;
    }

    public void UpdateTreasureBar(float value)
    {
        treasureBar.value = value;
    }

    public void UpdateRadarBar(float value)
    {
        radarBar.value = value;
    }

    public void UpdateCoinValue(int value)
    {
        coinText.text = value.ToString();
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
