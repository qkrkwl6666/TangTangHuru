using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponOrbSlot : MonoBehaviour
{
    public EquipPopUp equipPopUp;
    public ItemSlotUI[] upgradeSlots;
    public OrbUpgrader orbUpgrader;
    public OrbPanel orbPanel;

    private bool resetOn = true;

    private ItemType currItemType;

    private void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
            upgradeSlots[i].gameObject.SetActive(false);
        }

        currItemType = equipPopUp.currentItem.ItemType;

        if ((int)currItemType > 6)
            return;

        var currItem = equipPopUp.currentItem as M_Weapon;

        if (resetOn)
        {
            foreach (var slot in upgradeSlots)
            {
                slot.GetComponent<Button>().onClick.AddListener(() => orbUpgrader.SlotSelected(slot));
            }
            if (currItem.orbs != null)
            {
                LoadEquippedOrbList();
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < (int)currItem.ItemTier)
            {
                upgradeSlots[i].gameObject.SetActive(true);
            }
        }

        resetOn = false;
    }

    private void OnDisable()
    {
        if ((int)currItemType > 6)
            return;

        var currWeapon = equipPopUp.currentItem as M_Weapon;

        foreach (var slot in upgradeSlots)
        {
            if(slot.currItem == null)
                continue;
        }
    }

    public void AddOrbToWeapon(ItemSlotUI slot)
    {
        //var orbData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(slot.currItemId.ToString());


        //var selectedOrb = equipPopUp.mainInventory.GetItemTypesTier((ItemType)orbData.Item_Type, (ItemTier)orbData.Item_Tier);

        var currWeapon = equipPopUp.currentItem as M_Weapon;
        if (currWeapon.orbs == null)
        {
            currWeapon.orbs = new();
        }
        currWeapon.orbs.Add(slot.currItem);
    }

    public void RemoveOrbFromWeapon(ItemSlotUI slot)
    {
        //var orbData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(slot.currItemId.ToString());
        //var selectedOrb = equipPopUp.mainInventory.GetItemTypesTier((ItemType)orbData.Item_Type, (ItemTier)orbData.Item_Tier);

        var currWeapon = equipPopUp.currentItem as M_Weapon;
        currWeapon.orbs.Remove(slot.currItem);
    }

    public void LoadEquippedOrbList()
    {
        var currWeapon = equipPopUp.currentItem as M_Weapon;

        List<OrbDesc> connectOrbList = new();

        foreach (var weaponOrb in currWeapon.orbs)
        {
            if(weaponOrb ==  null)
                continue;

            orbPanel.SetList();

            //안켜진 상태로 호출해서 다 꺼져있음
            foreach (var invenOrb in orbPanel.orbList)
            {
                if (weaponOrb.ItemId == invenOrb.orbId)
                {
                    connectOrbList.Add(invenOrb);
                }
            }
        }

        if (connectOrbList.Count == 0)
            return;


        for(int i = 0; i < connectOrbList.Count; i++)
        {
            upgradeSlots[0].SetOrbInfo(connectOrbList[i]);
        }

    }
}
