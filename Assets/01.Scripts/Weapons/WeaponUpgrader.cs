using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SkillUpgradeData;

public class WeaponUpgrader : MonoBehaviour
{
    //WeaponCreator로 '생성'만 하도록 하고, 스킬 리스트, 강화정보, 강화처리 등
    //생성 이후에 발생하는 일들을 여기에서 관리

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
        //스테이지 내의 스킬 데이터를 받아서 조정후 리턴한다. 이미 만들어진 무기에 반영되지는 않는다. 별도 적용 필요.
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
