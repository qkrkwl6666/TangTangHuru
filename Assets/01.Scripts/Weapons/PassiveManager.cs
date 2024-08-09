using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PassiveManager : MonoBehaviour
{
    public List<WeaponCreator> weaponCreators;
    public List<WeaponCreator> currWeaponCreators;

    public List<PassiveData> passiveDataList;
    public List<PassiveData> inGamePassiveList = new(); //데이터 리스트 카피
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

        var subs = GameObject.FindGameObjectsWithTag("WeaponCreator");

        foreach( var sub in subs)
        {
            currWeaponCreators.Add(sub.GetComponent<WeaponCreator>());
            //To-Do. 아직 여기서 제거가 안됨. 
            weaponCreators.Remove(sub.GetComponent<WeaponCreator>()); 
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
        var parent = GetComponentInParent<PlayerController>().gameObject;

        var handle = Addressables.InstantiateAsync(item.itemData.Prefab_Id, parent.transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject mainWeapon = obj.Result;
                currWeaponCreators.Add(mainWeapon.GetComponent<WeaponCreator>());
            }
            else
            {
                Debug.LogError("Failed to instantiate the weapon.");
            }
        };
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
        Debug.LogError("패시브 강화 선택지 오류. 강화가능 목록에 없음.");
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
