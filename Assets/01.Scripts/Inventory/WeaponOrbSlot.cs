using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponOrbSlot : MonoBehaviour
{
    public EquipPopUp equipPopUp;
    public ItemSlotUI[] upgradeSlots;
    public OrbUpgrader orbUpgrader;
    private void OnEnable()
    {
        upgradeSlots = GetComponentsInChildren<ItemSlotUI>();

        foreach (var slot in upgradeSlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(() => orbUpgrader.SlotSelected(slot));
        }
    }

    private void OnDisable()
    {
        var currWeapon = equipPopUp.currentItem as M_Weapon;

        foreach(var slot in upgradeSlots)
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
