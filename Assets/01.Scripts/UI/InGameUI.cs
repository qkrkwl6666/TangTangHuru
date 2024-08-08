using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour, IPlayerObserver
{
    public GameObject treasurePrefab;

    private Transform playerTransform;
    private PlayerSubject playerSubject;

    // 보물 상자 흭득 UI
    private Slider treasureBar;
    // 레이더 UI
    public Slider radarBar;

    // 경험치 UI
    public Slider expBar;
    // 보스 체력 UI
    public Slider bossHpBar;
    // 코인 UI
    public GameObject coinUI;
    // 코인 Text
    public TextMeshProUGUI coinText;

    // 타이머 Text
    public TextMeshProUGUI stageTimer;

    // 게임 클리어 UI 
    public GameObject clearUI;

    // 조이스틱 UI
    public GameObject joystickUI;

    // 게임 클리어 UI
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI killText;

    // 설정 UI
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
        goldText.text = $"획득한 골드 : {gold.ToString()}";
        killText.text = $"처치한 몬스터 수 : {kill.ToString()}";
    }

    #region 일시정지
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
