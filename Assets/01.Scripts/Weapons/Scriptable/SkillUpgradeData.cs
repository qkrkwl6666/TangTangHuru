using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Upgrade Data", menuName = "Scriptable Object/Skill Upgrade Data", order = int.MaxValue)]
public class SkillUpgradeData : ScriptableObject
{
    public enum SkillUp
    {
        Damage,
        Speed,
        Range,
        CoolDown,
        BurstCount,
        PierceCount,
        LifeTime,
    }

    [SerializeField]
    private string weaponName;
    public string WeaponName { get { return weaponName; } }

    [SerializeField]
    private SkillUp[] level2_Upgrade;
    public SkillUp[] Level2_Upgrade { get { return level2_Upgrade; } }

    [SerializeField]
    private float[] level2_Value;
    public float[] Level2_Value { get { return level2_Value; } }

    [SerializeField]
    private SkillUp[] level3_Upgrade;
    public SkillUp[] Level3_Upgrade { get { return level3_Upgrade; } }

    [SerializeField]
    private float[] level3_Value;
    public float[] Level3_Value { get { return level3_Value; } }

    [SerializeField]
    private SkillUp[] level4_Upgrade;
    public SkillUp[] Level4_Upgrade { get { return level4_Upgrade; } }

    [SerializeField]
    private float[] level4_Value;
    public float[] Level4_Value { get { return level4_Value; } }




}
