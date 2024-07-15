using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SkillUpgradeData;

public class WeaponUpgrader : MonoBehaviour
{
    //WeaponCreator�� '����'�� �ϵ��� �ϰ�, ��ų ����Ʈ, ��ȭ����, ��ȭó�� ��
    //���� ���Ŀ� �߻��ϴ� �ϵ��� ���⿡�� ����

    public SkillUpgradeData skillUpgradeData;
    public WeaponData finalWeaponData;

    private DataInStage finalData;

    void Start()
    {
        finalData = new DataInStage();

        finalData.currWeaponLevel = 5;
        finalData.damage = finalWeaponData.Damage;
        finalData.speed = finalWeaponData.Speed;
        finalData.range = finalWeaponData.Range;
        finalData.coolDown = finalWeaponData.CoolDown;
        finalData.burstCount = finalWeaponData.BurstCount;
        finalData.burstRate = finalWeaponData.BurstRate;
        finalData.pierceCount = finalWeaponData.PierceCount;
        finalData.lifeTime = finalWeaponData.LifeTime;
        finalData.criticalChance = finalWeaponData.CriticalChance;
        finalData.criticalValue = finalWeaponData.CriticalValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DataInStage UpgradeWeaponData(DataInStage dataInStage)
    {
        //�������� ���� ��ų �����͸� �޾Ƽ� ������ �����Ѵ�. �̹� ������� ���⿡ �ݿ������� �ʴ´�. ���� ���� �ʿ�.
        dataInStage.currWeaponLevel++;

        List<SkillUp> upgradeStats = new();
        List<float> upgradeValue = new();

        switch (dataInStage.currWeaponLevel)
        {
            case 1:
                return dataInStage;
            case 2:
                upgradeStats = skillUpgradeData.Level2_Upgrade;
                upgradeValue = skillUpgradeData.Level2_Value;
                break;
            case 3: 
                upgradeStats = skillUpgradeData.Level3_Upgrade;
                upgradeValue = skillUpgradeData.Level3_Value;
                break;
            case 4:
                upgradeStats = skillUpgradeData.Level4_Upgrade;
                upgradeValue = skillUpgradeData.Level3_Value;
                break;
            case 5:
                return finalData;
        }


        for(int i = 0; i < upgradeStats.Count; ++i)
        {
            switch (upgradeStats[i])
            {
                case SkillUp.Damage:
                    dataInStage.damage = upgradeValue[i];
                    break;
                case SkillUp.Speed:
                    dataInStage.speed = upgradeValue[i];
                    break;
                case SkillUp.Range:
                    dataInStage.range = upgradeValue[i];
                    break;
                case SkillUp.CoolDown:
                    dataInStage.coolDown = upgradeValue[i];
                    break;
                case SkillUp.BurstCount:
                    dataInStage.burstCount = (int)upgradeValue[i];
                    break;
                case SkillUp.PierceCount:
                    dataInStage.pierceCount = (int)upgradeValue[i];
                    break;
                case SkillUp.LifeTime:
                    dataInStage.lifeTime = upgradeValue[i];
                    break;

            }
        }

        return dataInStage;
    }

    public void UpdataWeaponData(List<GameObject> weapons)
    {
        foreach (var weapon in weapons)
        {
            //var aimer = weapon.GetComponent<IAimer>();
            //aimer.Speed = weaponDataInStage.Speed;
            //aimer.LifeTime = weaponDataInStage.LifeTime;
            //hit.damage = weaponDataInStage.Damage;
            //hit.pierceCount = weaponDataInStage.PierceCount;
            //hit.criticalChance = weaponDataInStage.CriticalChance;
            //hit.criticalValue = weaponDataInStage.CriticalValue;
        }
    }
}
