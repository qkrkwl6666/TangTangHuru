using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrbUpgrader : MonoBehaviour
{
    public ItemSlotUI[] upgradeSlots;
    public Button upgradeButton;
    public OrbList popUp_OrbList;
    public GameObject popUp_Notice;

    public Image prefab_Orb;
    public Image prefab_RareOrb;
    public Image prefab_EpicOrb;

    private int firstItemId;

    private void OnEnable()
    {
        upgradeSlots = GetComponentsInChildren<ItemSlotUI>();

        foreach (var slot in upgradeSlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() => PopUpOrbList(slot));
        }

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void OnDisable()
    {
    }

    private void PopUpOrbList(ItemSlotUI slot)
    {
        if(slot.currItemId != 0)
        {
            slot.currItemId = 0;
            slot.slotIcon.gameObject.SetActive(false);
        }

        popUp_OrbList.gameObject.SetActive(true);
        popUp_OrbList.currSlot = slot;
    }

    private void Upgrade()
    {
        if (!CheckUpgradable())
            return;

        GameManager.Instance.currSaveData.orb_Normal -= 3;
        GameManager.Instance.currSaveData.orb_Rare++;
        popUp_OrbList.ResetOn();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].slotIcon.gameObject.SetActive(false);
        }
        popUp_Notice.SetActive(true);
    }

    private bool CheckUpgradable()
    {
        if (upgradeSlots == null || upgradeSlots.Length == 0)
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
