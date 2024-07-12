using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrader : MonoBehaviour
{
    //WeaponCreator�� '����'�� �ϵ��� �ϰ�, ��ų ����Ʈ, ��ȭ����, ��ȭó�� ��
    //���� ���Ŀ� �߻��ϴ� �ϵ��� ���⿡�� ����

    public SkillUpgradeData skillUpgradeData;
    public WeaponData finalWeaponData;

    private int currWeaponLevel = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeWeaponData()
    {
        currWeaponLevel++;

        SkillUpgradeData.SkillUp[] toUpgrades;

        switch (currWeaponLevel)
        {
            case 1:
                break;
            case 2:
                toUpgrades = skillUpgradeData.Level2_Upgrade;
                break;
            case 3:
                toUpgrades = skillUpgradeData.Level3_Upgrade;
                break;
            case 4:
                toUpgrades = skillUpgradeData.Level4_Upgrade;
                break;
            case 5:
                break;
        }
    }

    public void UpdataWeaponData()
    {
        //foreach (var weapon in weapons)
        //{
            //var aimer = weapon.GetComponent<IAimer>();
            //aimer.Speed = weaponDataInStage.Speed;
            //aimer.LifeTime = weaponDataInStage.LifeTime;
            //hit.damage = weaponDataInStage.Damage;
            //hit.pierceCount = weaponDataInStage.PierceCount;
            //hit.criticalChance = weaponDataInStage.CriticalChance;
            //hit.criticalValue = weaponDataInStage.CriticalValue;
        //}
    }
}
