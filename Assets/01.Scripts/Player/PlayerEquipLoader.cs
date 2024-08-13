using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerEquipLoader : MonoBehaviour
{
    private PassiveManager myPassiveManager;
    private PlayerHealth playerHealth;
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

            //무기에 서브무기가 되면 변경예정.
            M_Weapon weapon = GameManager.Instance.playerEquipment[PlayerEquipment.Weapon].Item1 as M_Weapon;
            WeaponAdd(weapon);
            var subWeaponList = weapon.GetSubWeapon();

            //장비 패시브
            var equipPassive = Instantiate(myPassiveManager.emptyPassiveData);
            M_Armour armor = GameManager.Instance.playerEquipment[PlayerEquipment.Armor].Item1 as M_Armour;
        }
        else
        {
            Debug.LogError("No PassiveManager!");
        }
    }

    public void WeaponAdd(M_Weapon item)
    {
        var handle = Addressables.InstantiateAsync(item.itemData.Prefab_Id, transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject mainWeapon = obj.Result;
                myPassiveManager.currWeaponCreators.Add(mainWeapon.GetComponent<WeaponCreator>());
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

}
