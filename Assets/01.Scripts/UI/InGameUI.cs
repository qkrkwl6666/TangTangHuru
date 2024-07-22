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
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        treasureBar = Instantiate(treasurePrefab, playerTransform).GetComponentInChildren<Slider>();
        treasureBar.gameObject.SetActive(false);
    }

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
