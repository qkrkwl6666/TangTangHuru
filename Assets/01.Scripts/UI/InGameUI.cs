using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour, IPlayerObserver
{
    private Transform playerTransform;
    private PlayerSubject playerSubject;

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

    #region ���� Ŭ���� 
    // ���� Ŭ���� UI
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI killText;

    public List<GameObject> itemSlot;

    #endregion

    // ���� UI
    public GameObject pauseUI;

    public GameObject bagUI;

    private void Awake()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
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

        int reinforcedStoneCount = 0;

        int reinforcedStoneId = 0;

        foreach (var inGameItem in GameManager.Instance.inGameItems)
        {
            switch (inGameItem.ItemType)
            {
                case IItemType.EquipmentGemstone: // ��� ����
                    foreach (var itemSlot in itemSlot)
                    {
                        if (itemSlot.activeSelf) continue;

                        var itemData = DataTableManager.Instance.Get<ItemTable>
                            (DataTableManager.item).GetItemData(inGameItem.ItemId.ToString());

                        itemSlot.GetComponentInChildren<M_UISlot>().SetItemData(itemData);
                        itemSlot.SetActive(true);
                        break;
                    }
                    break;
                case IItemType.ReinforcedStone: // ��ȭ��
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
