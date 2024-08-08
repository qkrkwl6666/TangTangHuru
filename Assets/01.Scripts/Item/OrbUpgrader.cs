using UnityEngine;
using UnityEngine.UI;

public class OrbUpgrader : MonoBehaviour
{
    public ItemSlotUI[] upgradeSlots;
    public Button upgradeButton;
    public OrbPanel popUp_OrbPanel;
    public GameObject popUp_Notice;

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

        GameManager.Instance.currSaveData.orb_Atk_Rare -= 3;
        GameManager.Instance.currSaveData.orb_Atk_Epic++;

        popUp_OrbPanel.ResetOn();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].ClearInfo();
        }
        popUp_Notice.SetActive(true);
    }

    private bool CheckUpgradable()
    {
        if (upgradeSlots == null || upgradeSlots.Length < 2)
            return false;

        firstItemId = upgradeSlots[0].currItemId;

        for (int i = 1; i < upgradeSlots.Length; i++)
        {
            if (upgradeSlots[i].currItemId != firstItemId)
            {
                Debug.Log("배치된 오브가 모두 같아야 한다.");
                return false;
            }
        }

        return true;
    }



}
