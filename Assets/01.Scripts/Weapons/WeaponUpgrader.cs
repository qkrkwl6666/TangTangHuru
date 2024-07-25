using System.Collections.Generic;
using UnityEngine;
using static SkillUpgradeData;

public class WeaponUpgrader : MonoBehaviour
{
    public SkillUpgradeData skillUpgradeData;
    public WeaponCreator firstWeaponCreator;
    public WeaponCreator finalWeaponCreator;


    public WeaponData UpgradeWeaponData(WeaponData dataInStage)
    {
        //스테이지 내의 스킬 데이터를 받아서 조정후 리턴한다. 이미 만들어진 무기에 반영되지는 않는다. 별도 적용 필요.
        List<SkillUp> upgradeStats = new();
        List<float> upgradeValue = new();

        switch (dataInStage.Level)
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
                upgradeValue = skillUpgradeData.Level4_Value;
                break;
            case 5:
                return dataInStage;
        }


        for (int i = 0; i < upgradeStats.Count; ++i)
        {
            switch (upgradeStats[i])
            {
                case SkillUp.Damage:
                    dataInStage.Damage = upgradeValue[i];
                    break;
                case SkillUp.Speed:
                    dataInStage.Speed = upgradeValue[i];
                    break;
                case SkillUp.Range:
                    dataInStage.Range = upgradeValue[i];
                    break;
                case SkillUp.CoolDown:
                    dataInStage.CoolDown = upgradeValue[i];
                    break;
                case SkillUp.BurstCount:
                    dataInStage.BurstCount = (int)upgradeValue[i];
                    break;
                case SkillUp.PierceCount:
                    dataInStage.PierceCount = (int)upgradeValue[i];
                    break;
                case SkillUp.LifeTime:
                    dataInStage.LifeTime = upgradeValue[i];
                    break;
                case SkillUp.Size:
                    dataInStage.Size = upgradeValue[i];
                    break;

            }
        }
        return dataInStage;
    }


    public void Evolution(List<GameObject> weapons)
    {
        switch (skillUpgradeData.Level5_Type)
        {
            case EvolutionType.Add:
                finalWeaponCreator.enabled = true;
                break;

            case EvolutionType.Replace:
                foreach (var weapon in weapons)
                {
                    Destroy(weapon);
                }
                weapons.Clear();
                firstWeaponCreator.enabled = false;
                finalWeaponCreator.enabled = true;
                break;
        }
    }
}
