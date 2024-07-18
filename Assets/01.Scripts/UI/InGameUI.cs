using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour, IPlayerObserver
{
    public GameObject treasurePrefab;
    public GameObject radarPrefab;

    private Transform playerTransform;
    private PlayerSubject playerSubject;

    // ∫∏π∞ ªÛ¿⁄ ≈âµÊ UI
    private Slider treasureBar;
    // ∑π¿Ã¥ı UI
    private Slider radarBar;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);

        treasureBar = Instantiate(treasurePrefab, playerTransform).GetComponentInChildren<Slider>();
        treasureBar.gameObject.SetActive(false);

        radarBar = Instantiate(radarPrefab, transform).GetComponentInChildren<Slider>();
        //radarBar.gameObject.SetActive(false);
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
