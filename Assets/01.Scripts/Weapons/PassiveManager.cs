using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PassiveManager : MonoBehaviour
{
    public List<WeaponCreator> weaponCreators;
    public List<WeaponCreator> currWeaponCreators;

    public List<PassiveData> passiveDataList;
    public List<PassiveData> inGamePassiveList = new(); //������ ����Ʈ ī��
    public List<PassiveData> currPassiveList = new();

    public PassiveData emptyPassiveData;

    private WeaponCreator currMainWeapon;
    private WeaponCreator[] currSubWeapon;
    private PassiveData totalPowerPassive;
    private PassiveData totalSpeedPassive;
    private PassiveData totalNoneTypePassive;

    private ItemData itemData;

    void Start()
    {
        WeaponAdd(GameManager.Instance.playerEquipment[PlayerEquipment.Weapon].Item1);

        currMainWeapon = GameObject.FindGameObjectWithTag("MainWeapon").GetComponent<WeaponCreator>();
        var subs = GameObject.FindGameObjectsWithTag("WeaponCreator");

        foreach( var sub in subs)
        {
            currWeaponCreators.Add(sub.GetComponent<WeaponCreator>());
            //To-Do. ���� ���⼭ ���Ű� �ȵ�. 
            weaponCreators.Remove(sub.GetComponent<WeaponCreator>()); 
        }
        if(currMainWeapon == null)
        {
            Debug.LogError("No Main Weapon!");
        }
        else
        {
            currWeaponCreators.Add(currMainWeapon);
        }


        for (int i = 0; i < passiveDataList.Count; i++)
        {
            inGamePassiveList.Add(Instantiate(passiveDataList[i]));
        }
        totalPowerPassive = Instantiate(emptyPassiveData);
        totalSpeedPassive = Instantiate(emptyPassiveData);
        totalNoneTypePassive = Instantiate(emptyPassiveData);


    }

    public void WeaponAdd(Item item)
    {
        Addressables.InstantiateAsync(item.itemData.Prefab_Id);
    }

    public void PassiveAdd(PassiveData selected)
    {
        var seletedPassive = selected;
        seletedPassive.Level = 1;
        currPassiveList.Add(seletedPassive);

        inGamePassiveList.Remove(selected);
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
        Debug.LogError("�нú� ��ȭ ������ ����. ��ȭ���� ��Ͽ� ����.");
    }

    public void PassiveEquip()
    {
        foreach (var weaponCreator in currWeaponCreators)
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
}
