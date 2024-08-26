using System.Collections.Generic;
using UnityEngine;

public class CurrWeaponViewer : MonoBehaviour
{
    public IconLoader iconLoader;
    public List<CurrWeaponEntry> weaponEntries;
    PassiveManager passiveManager;
    List<WeaponCreator> currWeapons = new List<WeaponCreator>();


    private void OnEnable()
    {
        if(passiveManager ==  null)
        {
            passiveManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PassiveManager>();
        }

        currWeapons = passiveManager.currWeaponCreators;

        for(int i = 0; i < currWeapons.Count; i++)
        {
            var icon = iconLoader.SetIconByName("Icon" + currWeapons[i].weaponDataRef.WeaponName);

            weaponEntries[i].SetInfo(icon, currWeapons[i].currLevel);
        }
    }
}
