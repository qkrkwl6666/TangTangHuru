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
    private WeaponData.Type mainType;
    private int setType = 0;

    private float hpValue = 0;
    private float defValue = 0;
    private float dodgeValue = 0;


    private void Awake()
    {
        myPassiveManager = GetComponentInChildren<PassiveManager>();
        playerHealth = GetComponent<PlayerHealth>();

        if (myPassiveManager != null)
        {
            Item equipHp = null;
            Item equipDef = null;
            Item equipDodge = null;

            if (GameManager.Instance.playerEquipment.TryGetValue(PlayerEquipment.Armor, out var armor))
            {
                equipHp = armor.Item1;
                if (equipHp != null)
                {
                    hpValue = equipHp.itemData.Hp;
                }
            }
            if (GameManager.Instance.playerEquipment.TryGetValue(PlayerEquipment.Helmet, out var helmet))
            {
                equipDef = helmet.Item1;
                if (equipDef != null)
                {
                    defValue = equipDef.itemData.Defense;
                }
            }
            if (GameManager.Instance.playerEquipment.TryGetValue(PlayerEquipment.Shoes, out var shoes))
            {
                equipDodge = shoes.Item1;
                if (equipDodge != null)
                {
                    dodgeValue = equipDodge.itemData.Dodge;
                }
            }


            if (equipHp != null && equipDef != null && equipDodge != null)
            {
                var armorSetType = equipHp.itemData.SetType;
                if (armorSetType == equipDef.itemData.SetType && armorSetType == equipDodge.itemData.SetType)
                {
                    setType = armorSetType; //세트효과 적용
                }
            }

        }

        if (myPassiveManager != null)
        {
            if (GameManager.Instance.playerEquipment.ContainsKey(PlayerEquipment.Pet))
            {
                PetAdd(GameManager.Instance.playerEquipment[PlayerEquipment.Pet].Item1);
            }

            if (GameManager.Instance.playerEquipment.ContainsKey(PlayerEquipment.Weapon))
            {
                M_Weapon mainWeapon = GameManager.Instance.playerEquipment[PlayerEquipment.Weapon].Item1 as M_Weapon;
                mainDmg = mainWeapon.itemData.Damage;
                mainCoolDown = mainWeapon.itemData.CoolDown;
                mainCriticalChance = mainWeapon.itemData.CriticalChance;
                mainCriticalValue = mainWeapon.itemData.Criticaldam;

                if (mainWeapon.orbs != null)
                {
                    foreach (var orb in mainWeapon.orbs)
                    {
                        switch (orb.ItemType)
                        {
                            case ItemType.OrbAttack:
                                mainDmg += orb.itemData.Damage;
                                break;
                            case ItemType.OrbHp:
                                hpValue += orb.itemData.Hp;
                                break;
                            case ItemType.OrbDefence:
                                defValue += orb.itemData.Defense;
                                break;
                            case ItemType.OrbDodge:
                                dodgeValue += orb.itemData.Dodge;
                                break;
                        }
                    }
                }

                playerHealth.SetInfo(hpValue, defValue, dodgeValue);
                MainWeaponAdd(mainWeapon);

                var subWeaponList = mainWeapon.subWeapons;
                if (subWeaponList != null)
                {
                    foreach (var sub in subWeaponList)
                    {
                        SubWeaponAdd(sub);
                    }
                }
            }
            else
            {
                mainDmg = 50;
                mainCoolDown = 1.5f;
                mainCriticalChance = 0;
                mainCriticalValue = 2;
                DefaultWeapon("Weapon Sword");
            }
        }
        else
        {
            //Debug.LogError("No PassiveManager!");
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
                var weaponCreators = mainWeapon.GetComponents<WeaponCreator>();
                mainType = weaponCreators[0].weaponDataRef.WeaponType;

                foreach (var weaponCreator in weaponCreators)
                {
                    weaponCreator.SetMainInfo(mainDmg, mainCoolDown, mainCriticalChance, mainCriticalValue, mainType);
                }
                weaponCreators[0].isMainWeapon = true;
                myPassiveManager.currWeaponCreators.Insert(0, weaponCreators[0]);
                mainWeaponCreator = weaponCreators[0];
            }
            else
            {
                //Debug.LogError("Failed to instantiate the weapon.");
            }
        };
    }

    public void DefaultWeapon(string prefabId)
    {
        var handle = Addressables.InstantiateAsync(prefabId, transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject mainWeapon = obj.Result;
                var weaponCreators = mainWeapon.GetComponents<WeaponCreator>();
                mainType = weaponCreators[0].weaponDataRef.WeaponType;

                foreach (var weaponCreator in weaponCreators)
                {
                    weaponCreator.SetMainInfo(mainDmg, mainCoolDown, mainCriticalChance, mainCriticalValue, mainType);
                }
                weaponCreators[0].isMainWeapon = true;
                myPassiveManager.currWeaponCreators.Add(weaponCreators[0]);
                mainWeaponCreator = weaponCreators[0];
            }
            else
            {
                //Debug.LogError("Failed to instantiate the weapon.");
            }
        };
    }

    public void SubWeaponAdd(ItemData subData)
    {
        var handle = Addressables.InstantiateAsync(subData.Prefab_Id, transform);
        handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject weapon = obj.Result;
                var weaponCreators = weapon.GetComponents<WeaponCreator>();
                mainType = weaponCreators[0].weaponDataRef.WeaponType;

                foreach (var weaponCreator in weaponCreators)
                {
                    weaponCreator.SetMainInfo(mainDmg, mainCoolDown, mainCriticalChance, mainCriticalValue, mainType);
                }
                myPassiveManager.currWeaponCreators.Add(weapon.GetComponent<WeaponCreator>());
                myPassiveManager.RemoveFromUnoptainWeaponList(weapon.GetComponent<WeaponCreator>());
            }
            else
            {
                //Debug.LogError("Failed to instantiate the weapon.");
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
                //Debug.Log("Succeed to instantiate the pet.");
            }
            else
            {
                //Debug.Log("Failed to instantiate the pet.");
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
                //Debug.Log("Succeed to instantiate the pet.");
            }
            else
            {
                // Debug.Log("Failed to instantiate the pet.");
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

    public int GetArmorSetType()
    {
        return setType;
    }
}
