using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    [SerializeField]
    private List<WeaponCreator> weaponCreators;

    public List<PassiveData> passiveDataList;
    private List<PassiveData> currPassiveList;

    PassiveData totalPowerPassive;
    PassiveData totalSpeedPassive;
    PassiveData totalNoneTypePassive;


    void Start()
    {

    }
    public void PassiveAdd()
    {
        currPassiveList.Add(passiveDataList[0]);
        PassiveTotal();
        PassiveEquip();
    }

    public void PassiveEquip()
    {
        foreach (var weaponCreator in weaponCreators)
        {
            switch (weaponCreator.weaponDataRef.WeaponType)
            {
                case WeaponData.Type.PowerType:
                    weaponCreator.typePassive = totalPowerPassive;
                    break;
                case WeaponData.Type.SpeedType:
                    weaponCreator.typePassive = totalSpeedPassive;
                    break;
            }

            weaponCreator.commonPassive = totalNoneTypePassive;
        }
    }

    public void PassiveTotal()
    {
        totalPowerPassive = new PassiveData();
        totalSpeedPassive = new PassiveData();
        totalNoneTypePassive = new PassiveData();

        foreach (var passiveData in currPassiveList)
        {
            switch(passiveData.ItemType)
            {
                case PassiveData.PassiveType.PowerType:
                    totalPowerPassive.Damage += passiveData.Damage * passiveData.Level;
                    totalPowerPassive.CoolDown += passiveData.CoolDown * passiveData.Level;
                    break;
                case PassiveData.PassiveType.SpeedType:
                    totalSpeedPassive.Damage += passiveData.Damage * passiveData.Level;
                    totalSpeedPassive.CoolDown += passiveData.CoolDown * passiveData.Level;
                    break;
                case PassiveData.PassiveType.None:
                    totalNoneTypePassive.CriticalChance += passiveData.CriticalChance * passiveData.Level;
                    totalNoneTypePassive.CriticalValue += passiveData.CriticalValue * passiveData.Level;
                    break;
            }
        }
    }
    public void PassiveUpgrade(int num)
    {
        currPassiveList[num].Level++;
    }

    void Update()
    {
        
    }
}
