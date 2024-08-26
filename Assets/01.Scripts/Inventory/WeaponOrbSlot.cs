using UnityEngine;
using UnityEngine.UI;

public class WeaponOrbSlot : MonoBehaviour
{
    public EquipPopUp equipPopUp;
    public ItemSlotUI[] upgradeSlots;
    public OrbUpgrader orbUpgrader;


    private void OnEnable()
    {
        var currWeapon = equipPopUp.currentItem as M_Weapon;

        foreach (var slot in upgradeSlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() => orbUpgrader.SlotSelected(slot));
        }

        for(int i = 0; i < 4; i++)
        {
            if(i < (int)currWeapon.ItemTier)
            {
                upgradeSlots[i].gameObject.SetActive(true);
            }
            else
            {
                upgradeSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        var currWeapon = equipPopUp.currentItem as M_Weapon;

        foreach (var slot in upgradeSlots)
        {
            if(slot.currItemId == 0)
                continue;

            currWeapon.orbs.Add(equipPopUp.mainInventory.MainInventoryAddItem(slot.currItemId.ToString(), 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
