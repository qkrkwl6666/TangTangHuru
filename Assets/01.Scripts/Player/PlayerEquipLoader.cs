using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerEquipLoader : MonoBehaviour
{
    private PassiveManager myPassiveManager;
    private PlayerHealth playerHealth;
    private WeaponCreator mainWeaponCreator;
    private float mainDmg;
    private float mainCoolDown;
    private float mainCriticalChance;
    private float mainCriticalValue;

    private void Start()
    {
        myPassiveManager = GetComponentInChildren<PassiveManager>();
        playerHealth = GetComponent<PlayerHealth>();

        if(myPassiveManager != null)
        {
            var equipHp = GameManager.Instance.playerEquipment[PlayerEquipment.Armor].Item1;
            var equipDef = GameManager.Instance.playerEquipment[PlayerEquipment.Helmet].Item1;
            var equipDodge = GameManager.Instance.playerEquipment[PlayerEquipment.Shoes].Item1;

            playerHealth.SetInfo(equipHp.itemData.Hp, equipDef.itemData.Defense, equipDodge.itemData.Dodge);
        }

        if (myPassiveManager != null)
        {
            if (GameManager.Instance.playerEquipment.ContainsKey(PlayerEquipment.Pet))
            {
                PetAdd(GameManager.Instance.playerEquipment[PlayerEquipment.Pet].Item1);
            }

            //무기에 서브무기가 들어가면 변경예정.
            M_Weapon mainWeapon = GameManager.Instance.playerEquipment[PlayerEquipment.Weapon].Item1 as M_Weapon;
            mainDmg = mainWeapon.itemData.Damage;
            mainCoolDown = mainWeapon.itemData.CoolDown;
            mainCriticalChance = mainWeapon.itemData.CriticalChance;
            mainCriticalValue = mainWeapon.itemData.Criticaldam;
            MainWeaponAdd(mainWeapon);
            var subWeaponList = mainWeapon.GetSubWeapon();
        }
        else
        {
            Debug.LogError("No PassiveManager!");
        }
    }

    public void MainWeaponAdd(M_Weapon item)
    {
        var handle = Addressables.InstantiateAsync(item.itemData.Prefab_Id, transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject mainWeapon = obj.Result;
                var weaponCreator = mainWeapon.GetComponent<WeaponCreator>();
                var mainType = weaponCreator.weaponDataRef.WeaponType;

                weaponCreator.SetMainInfo(mainDmg, mainCoolDown, mainCriticalChance, mainCriticalValue, mainType);
                myPassiveManager.currWeaponCreators.Add(weaponCreator);
                mainWeaponCreator = weaponCreator;
            }
            else
            {
                Debug.LogError("Failed to instantiate the weapon.");
            }
        };
    }

    public void SubWeaponAdd(M_Weapon item)
    {
        var handle = Addressables.InstantiateAsync(item.itemData.Prefab_Id, transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject weapon = obj.Result;
                var weaponCreator = weapon.GetComponent<WeaponCreator>();
                myPassiveManager.currWeaponCreators.Add(weaponCreator);
                mainWeaponCreator = weaponCreator;
            }
            else
            {
                Debug.LogError("Failed to instantiate the weapon.");
            }
        };
    }

    public void PetAdd(Item item)
    {
        var handle = Addressables.InstantiateAsync(item.itemData.Prefab_Id, transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Succeed to instantiate the pet.");
            }
            else
            {
                Debug.Log("Failed to instantiate the pet.");
            }
        };
    }

    public void PassiveAdd(PassiveData selected)
    {
        var handle = Addressables.InstantiateAsync(selected.ToString(), transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Succeed to instantiate the pet.");
            }
            else
            {
                Debug.Log("Failed to instantiate the pet.");
            }
        };

        var seletedPassive = selected;
        seletedPassive.Level = 1;
        
        //currPassiveList.Add(seletedPassive);
        //inGamePassiveList.Remove(selected);

        myPassiveManager.SetTotalPassive();
        myPassiveManager.PassiveEquip();
        return;
    }

    public WeaponCreator GetMainWeapon()
    {
        return mainWeaponCreator;
    }
}
