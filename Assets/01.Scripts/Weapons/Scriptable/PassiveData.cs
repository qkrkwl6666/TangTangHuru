using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Passive Data", menuName = "Scriptable Object/Passive Data", order = int.MaxValue)]
public class PassiveData : ScriptableObject
{
    public enum PassiveType
    {
        PowerType,
        SpeedType,
        None,
    }
    public enum PassiveUp
    {
        Health,
        Damage,
        Speed,
        Range,
        CoolDown,
        BurstCount,
        PierceCount,
        LifeTime,
    }

    [SerializeField]
    private string passiveName;
    public string PassiveName { get { return passiveName; } }

    [SerializeField]
    private PassiveType itemType;
    public PassiveType ItemType { get { return itemType; } }
    [SerializeField]
    private List<PassiveUp> passives;
    public List<PassiveUp> Passives { get { return passives; } }

    [SerializeField]
    private List<float> passives_Value;
    public List<float> Passives_Value { get { return passives_Value; } }
}
