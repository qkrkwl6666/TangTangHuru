using Spine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponOrbSlot : MonoBehaviour
{
    public EquipPopUp equipPopUp;
    public ItemSlotUI[] upgradeSlots;
    public OrbUpgrader orbUpgrader;

    private bool resetOn = true;

    private void OnEnable()
    {
        var currWeapon = equipPopUp.currentItem as M_Weapon;

        if (resetOn)
        {
            foreach (var slot in upgradeSlots)
            {
                slot.GetComponent<Button>().onClick.AddListener(() => orbUpgrader.SlotSelected(slot));
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < (int)currWeapon.ItemTier)
            {
                upgradeSlots[i].gameObject.SetActive(true);
            }
            else
            {
                upgradeSlots[i].gameObject.SetActive(false);
            }
        }

        resetOn = false;
    }

    private void OnDisable()
    {
        var currWeapon = equipPopUp.currentItem as M_Weapon;

        foreach (var slot in upgradeSlots)
        {
            if(slot.currItemId == 0)
                continue;

        }
    }

    public void AddOrbToWeapon(ItemSlotUI slot)
    {
        var orbData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(slot.currItemId.ToString());

        var currWeapon = equipPopUp.currentItem as M_Weapon;
        var selectedOrb = equipPopUp.mainInventory.GetItemTypesTier((ItemType)orbData.Item_Type, (ItemTier)orbData.Item_Tier);

        if(currWeapon.orbs == null)
        {
            currWeapon.orbs = new();
        }
        currWeapon.orbs.Add(selectedOrb[0]);
    }

    public void RemoveOrbFromWeapon(ItemSlotUI slot)
    {
        var orbData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(slot.currItemId.ToString());

        var currWeapon = equipPopUp.currentItem as M_Weapon;
        var selectedOrb = equipPopUp.mainInventory.GetItemTypesTier((ItemType)orbData.Item_Type, (ItemTier)orbData.Item_Tier);
        currWeapon.orbs.Remove(selectedOrb[0]);
    }
}
