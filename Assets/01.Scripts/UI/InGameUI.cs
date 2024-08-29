using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour, IPlayerObserver
{
    private Transform playerTransform;
    private PlayerSubject playerSubject;

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

    #region 게임 클리어 
    // 게임 클리어 UI
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI killText;

    public List<GameObject> itemSlot;

    #endregion

    // 설정 UI
    public GameObject pauseUI;
    public GameObject powerImage;
    public GameObject speedImage;

    public GameObject bagUI;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
    }
    private void Start()
    {
        //if (GameManager.Instance.playerEquipment.ContainsKey(PlayerEquipment.Weapon))
        //{
        //    switch (GameManager.Instance.playerEquipment[PlayerEquipment.Weapon].Item1.ItemType)
        //    {
        //        case ItemType.Axe:
        //        case ItemType.Crossbow:
        //        case ItemType.Staff:
        //            powerImage.SetActive(true);
        //            levelUpPowerImage.SetActive(true);
        //            break;
        //        case ItemType.Sword:
        //        case ItemType.Bow:
        //        case ItemType.Wand:
        //            speedImage.SetActive(true);
        //            levelUpSpeedImage.SetActive(true);
        //            break;
        //    }
        //}
        //else
        //{
        //    speedImage.SetActive(true);
        //}
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

        int reinforcedStoneCount = 0;

        int reinforcedStoneId = 0;

        foreach (var inGameItem in GameManager.Instance.inGameItems)
        {
            switch (inGameItem.ItemType)
            {
                case IItemType.EquipmentGemstone: // 장비 원석
                    foreach (var itemSlot in itemSlot)
                    {
                        if (itemSlot.activeSelf) continue;

                        var itemData = DataTableManager.Instance.Get<ItemTable>
                            (DataTableManager.item).GetItemData(inGameItem.ItemId.ToString());

                        itemSlot.GetComponentInChildren<M_UISlot>().SetItemData(itemData);// 에러 나는곳
                        itemSlot.SetActive(true);
                        break;
                    }
                    break;
                case IItemType.ReinforcedStone: // 강화석
                    reinforcedStoneCount++;
                    reinforcedStoneId = inGameItem.ItemId;
                    break;
            }
        }

        if (reinforcedStoneCount == 0) return;

        foreach (var itemSlot in itemSlot)
        {
            if (itemSlot.activeSelf) continue;

            var itemData = DataTableManager.Instance.Get<ItemTable>
                (DataTableManager.item).GetItemData(reinforcedStoneId.ToString());

            itemSlot.GetComponentInChildren<M_UISlot>()
                .SetItemDataConsumable(itemData, reinforcedStoneCount);
            itemSlot.SetActive(true);
            break;
        }
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
        //테스트중
        StopAllCoroutines();
        GameManager.Instance.LoadSceneAsync(Defines.mainScene);
    }

    #endregion

    public void SetActiveBagUI(bool active)
    {
        bagUI.SetActive(active);
    }

    public void SetActiveExpBar(bool active)
    {
        expBar.gameObject.SetActive(active);
    }

    public void SetActiveBossHpBar(bool active)
    {
        bossHpBar.gameObject.SetActive(active);
    }

    public void UpdateBossHpBar(float value)
    {
        bossHpBar.value = value;
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
