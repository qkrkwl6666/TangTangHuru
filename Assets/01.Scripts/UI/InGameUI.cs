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
