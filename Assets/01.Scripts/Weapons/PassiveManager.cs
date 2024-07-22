using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    public List<PassiveData> passiveData;

    private WeaponCreator[] weaponCreatorList;

    void Start()
    {
        weaponCreatorList = GetComponentsInParent<WeaponCreator>();
    }

    public void PassiveEquip()
    {
        for (int i = 0; i < weaponCreatorList.Length; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
