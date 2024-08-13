using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbUpgrader : MonoBehaviour
{
    public MainInventory inventory;
    public ItemSlotUI[] upgradeSlots;
    public Button upgradeButton;
    public OrbPanel popUp_OrbPanel;
    public GameObject popUp_Notice;

    private ItemData orbData;

    private int firstItemId;

    private void OnEnable()
    {
        upgradeSlots = GetComponentsInChildren<ItemSlotUI>();

        foreach (var slot in upgradeSlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() => SlotSelected(slot));
        }

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void OnDisable()
    {
        foreach (var slot in upgradeSlots)
        {
            slot.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void SlotSelected(ItemSlotUI currSlot)
    {
        if(currSlot.isSelected)
        {
            UndoSelect(currSlot);
        }
        else
        {
            PopUpOrbPanel(currSlot);
        }

    }


    private void PopUpOrbPanel(ItemSlotUI slot)
    {
        popUp_OrbPanel.currSlot = slot;
        popUp_OrbPanel.gameObject.SetActive(true);
    }



    public void SelectOrbInPanel(int index)
    {
        popUp_OrbPanel.currSlot.SetOrbInfo(popUp_OrbPanel.orbList[index]);
        popUp_OrbPanel.gameObject.SetActive(false);
    }
    public void UndoSelect(ItemSlotUI slot)
    {
        slot.ClearInfo();
    }

    private void Upgrade()
    {
        if (!CheckUpgradable())
            return;

        //ToDo. RemoveItem 메소드 완성되면 재료소비 추가예정
        inventory.MainInventoryAddItem((orbData.Item_Id + 1).ToString());

        popUp_OrbPanel.ResetOn();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].ClearInfo();
        }

        orbData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData((firstItemId + 1).ToString());
        SetNoticePopUp();
    }

    private bool CheckUpgradable()
    {
        if (upgradeSlots == null || upgradeSlots.Length < 2)
            return false;

        firstItemId = upgradeSlots[0].currItemId;
        if (firstItemId == 0)
            return false;


        for (int i = 1; i < upgradeSlots.Length; i++)
        {
            if (upgradeSlots[i].currItemId != firstItemId)
            {
                Debug.Log("배치된 오브가 모두 같아야 한다.");
                return false;
            }
        }

        orbData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(firstItemId.ToString());

        if(orbData.Item_Tier > 3)
        {
            Debug.Log("이미 최고티어 오브이다.");
        }


        return true;
    }

    private void SetNoticePopUp()
    {
        var strTable = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);

        var desc = strTable.Get(orbData.Desc_Id).Text;
        var name = strTable.Get(orbData.Name_Id).Text;
        popUp_Notice.GetComponentInChildren<TextMeshProUGUI>().text = ($"{desc} {name} 오브 획득!");
        popUp_Notice.SetActive(true);
    }

}
