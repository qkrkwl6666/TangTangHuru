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

    [Header("�̸� �� Ÿ��")]
    [SerializeField]
    private string passiveName;
    public string PassiveName { get { return passiveName; } }

    [SerializeField]
    private PassiveType itemType;
    public PassiveType ItemType { get { return itemType; } }

    [Header("����")]
    [SerializeField]
    private string passiveDesc;
    public string PassiveDesc { get { return passiveDesc; } }

    [Header("����")]
    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }

    [Header("����� ũ��")]
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    [Header("�߻� ���ð� ���ҷ�")]
    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } set { coolDown = value; } }

    [Header("ġ��Ÿ ���� �׸�")]
    [SerializeField]
    private float criticalChance;
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [SerializeField]
    private float criticalValue;
    public float CriticalValue { get { return criticalValue; } set { criticalValue = value; } }


}
