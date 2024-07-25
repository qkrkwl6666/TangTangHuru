using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Upgrade Data", menuName = "Scriptable Object/Skill Upgrade Data", order = int.MaxValue)]
public class SkillUpgradeData : ScriptableObject
{
    public enum EvolutionType
    {
        Add,
        Replace,
    }

    public enum SkillUp
    {
        Damage,
        Speed,
        Range,
        CoolDown,
        BurstCount,
        PierceCount,
        LifeTime,
        Size,
    }

    [SerializeField]
    private string weaponName;
    public string WeaponName { get { return weaponName; } }

    [SerializeField]
    private List<SkillUp> level2_Upgrade;
    public List<SkillUp> Level2_Upgrade { get { return level2_Upgrade; } }

    [SerializeField]
    private List<float> level2_Value;
    public List<float> Level2_Value { get { return level2_Value; } }

    [SerializeField]
    private List<SkillUp> level3_Upgrade;
    public List<SkillUp> Level3_Upgrade { get { return level3_Upgrade; } }

    [SerializeField]
    private List<float> level3_Value;
    public List<float> Level3_Value { get { return level3_Value; } }

    [SerializeField]
    private List<SkillUp> level4_Upgrade;
    public List<SkillUp> Level4_Upgrade { get { return level4_Upgrade; } }

    [SerializeField]
    private List<float> level4_Value;
    public List<float> Level4_Value { get { return level4_Value; } }

    [SerializeField]
    private EvolutionType level5_Type;
    public EvolutionType Level5_Type { get { return level5_Type; } }

}
