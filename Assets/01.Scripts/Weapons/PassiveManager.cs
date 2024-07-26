using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    public List<WeaponCreator> weaponCreators;

    public List<PassiveData> passiveDataList;
    public List<PassiveData> currPassiveList = new();

    public PassiveData passiveData;

    public WeaponCreator currMainWeapon;

    PassiveData totalPowerPassive;
    PassiveData totalSpeedPassive;
    PassiveData totalNoneTypePassive;


    void Start()
    {
        currMainWeapon = GameObject.FindGameObjectWithTag("MainWeapon").GetComponent<WeaponCreator>();
        if(currMainWeapon == null)
        {
            Debug.LogError("No Main Weapon!");
        }
        totalPowerPassive = Instantiate(passiveData);
        totalSpeedPassive = Instantiate(passiveData);
        totalNoneTypePassive = Instantiate(passiveData);
        weaponCreators.Add(currMainWeapon);
    }
    public void PassiveAdd(PassiveData selected)
    {
        var seletedPassive = selected;
        seletedPassive.Level = 1;
        currPassiveList.Add(seletedPassive);

        passiveDataList.Remove(selected);
        SetTotalPassive();
        PassiveEquip();
        return;
    }
    public void PassiveLevelUp(PassiveData selected)
    {
        foreach (var currPassive in currPassiveList)
        {
            if (currPassive == selected)
            {
                currPassive.Level++;
                SetTotalPassive();
                PassiveEquip();
                return;
            }
        }
        Debug.LogError("패시브 강화 선택지 오류. 강화가능 목록에 없음.");
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
            switch (passiveData.ItemType)
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
