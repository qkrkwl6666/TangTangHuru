using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    [SerializeField]
    private List<WeaponCreator> weaponCreators;

    public List<PassiveData> passiveDataList;
    private List<PassiveData> currPassiveList = new();

    public PassiveData passiveData;
    
    PassiveData totalPowerPassive;
    PassiveData totalSpeedPassive;
    PassiveData totalNoneTypePassive;


    void Start()
    {
        totalPowerPassive = Instantiate(passiveData);
        totalSpeedPassive = Instantiate(passiveData);
        totalNoneTypePassive = Instantiate(passiveData);
    }
    public void PassiveAdd()
    {
        currPassiveList.Add(passiveDataList[0]);
        SetTotalPassive();
        PassiveEquip();
    }

    public void PassiveEquip()
    {
        foreach (var weaponCreator in weaponCreators)
        {
            switch (weaponCreator.weaponDataRef.WeaponType)
            {
                case WeaponData.Type.PowerType:
                    weaponCreator.SetPassive(totalPowerPassive, totalNoneTypePassive);
                    break;
                case WeaponData.Type.SpeedType:
                    weaponCreator.SetPassive(totalSpeedPassive, totalNoneTypePassive);
                    break;
            }
        }
    }

    public void SetTotalPassive()
    {
        ClearData(totalPowerPassive);
        ClearData(totalSpeedPassive);
        ClearData(totalNoneTypePassive);

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

    private void ClearData(PassiveData data)
    {
        data.Damage = 0;
        data.CoolDown = 0;
        data.CriticalChance = 0;
        data.CriticalValue = 0;
    }

    void Update()
    {
        
    }
}
