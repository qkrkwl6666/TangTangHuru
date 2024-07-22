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

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        treasureBar = Instantiate(treasurePrefab, playerTransform).GetComponentInChildren<Slider>();
        treasureBar.gameObject.SetActive(false);
    }

    public void SetActiveTreasureBar(bool show)
    {
        treasureBar.gameObject.SetActive(show);
    }

    public void UpdateTreasureBar(float value)
    {
        treasureBar.value = value;
    }

    public void UpdateRadarBar(float value)
    {
        radarBar.value = value;
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }
}
