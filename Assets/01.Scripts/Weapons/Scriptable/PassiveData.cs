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
        Damage,
        CoolDown,
        CriticalChance,
        CriticalValue,
        //Health,
        //Speed,
        //Range,
        //BurstCount,
        //PierceCount,
        //LifeTime,
    }

    [Header("이름 및 타입")]
    [SerializeField]
    private string passiveName;
    public string PassiveName { get { return passiveName; } }

    [SerializeField]
    private PassiveType itemType;
    public PassiveType ItemType { get { return itemType; } }

    [Header("설명")]
    [SerializeField]
    private string passiveDesc;
    public string PassiveDesc { get { return passiveDesc; } }

    [Header("레벨")]
    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }

    [Header("대미지 크기")]
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    [Header("발사 대기시간 감소량")]
    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } set { coolDown = value; } }

    [Header("치명타 관련 항목")]
    [SerializeField]
    private float criticalChance;
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [SerializeField]
    private float criticalValue;
    public float CriticalValue { get { return criticalValue; } set { criticalValue = value; } }


}
