using System.Runtime.Remoting.Messaging;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    public enum Type
    {
        PowerType,
        SpeedType,
    }
    public enum AimType
    {
        Auto,
        Manual,
        Fixed,
        Player,
        RandomTarget,
        RandomSeed,
        Angular,
        RandomPos,
    }
    public enum MoveType
    {
        Melee,
        Shoot,
        WaveShot,
        Rotate,
        Fixed,
        SpreadShot,
        SpreadWall,
        Laser,
        Spawn,
        Parabola,
        BackandForward,
        ParabolaRotate,
        Reflecting,
    }
    public enum AttackType
    {
        Enter,
        Stay,
        OneOff,
    }
    public enum Option
    {
        FadeInOut,
        SizeUp,
        SizeDown,
        Randomizer,
        SecondAttack,
    }


    [Header("�̸�, ����, Ÿ��")]
    [SerializeField]
    private string weaponName;
    public string WeaponName { get { return weaponName; } }

    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField]
    private Type weaponType;
    public Type WeaponType { get { return weaponType; } set { weaponType = value; } }

    [Header("���� ���")]
    [SerializeField]
    private AimType aimType;
    public AimType WeaponAimType { get { return aimType; } set { aimType = value; } }

    [Header("���� ���(���� �� ������)")]
    [SerializeField]
    private MoveType moveType;
    public MoveType WeaponMoveType { get { return moveType; } set { moveType = value; } }

    [Header("���� ���(�ٴ� ��Ʈ ����)")]
    [SerializeField]
    private AttackType attackType;
    public AttackType WeaponAttckType { get { return attackType; } set { attackType = value; } }

    [Header("(�ٴ� ��Ʈ�� ���) ���� ����")]
    [SerializeField]
    private float singleAttackRate;
    public float SingleAttackRate { get { return singleAttackRate; } set { singleAttackRate = value; } }

    [Header("����� ���� ��ġ")]
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }
    [SerializeField]
    private float criticalChance;
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [SerializeField]
    private float criticalValue;
    public float CriticalValue { get { return criticalValue; } set { criticalValue = value; } }

    [Header("���� �Ÿ�")]
    [SerializeField]
    private float range;
    public float Range { get { return range; } set { range = value; } }

    [Header("�˹� ũ��")]
    [SerializeField]
    private float impact;
    public float Impact { get { return impact; } set { impact = value; } }

    [Header("����ü �ӵ�")]
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [Header("����ü ũ��")]
    [SerializeField]
    private float size;
    public float Size { get { return size; } set { size = value; } }

    [Header("����ü �����ð�")]
    [SerializeField]
    private float lifeTime;
    public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }

    [Header("�߻� ���ð�")]
    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } set { coolDown = value; } }

    [Header("���� ���� ���� �� ����")]
    [SerializeField]
    private int burstCount;
    public int BurstCount { get { return burstCount; } set { burstCount = value; } }

    [SerializeField]
    private float burstRate;
    public float BurstRate { get { return burstRate; } set { burstRate = value; } }

    [Header("���� ȸ��")]
    [SerializeField]
    private int pierceCount;
    public int PierceCount { get { return pierceCount; } set { pierceCount = value; } }

    [Header("����ü �ΰ��ɼ�")]
    public Option[] Options;

    [Header("��ź ���� �ɼ� ��ü ����")]
    [SerializeField]
    private int secondAtkCount;
    public int SecondAtkCount { get { return secondAtkCount; } set { secondAtkCount = value; } }
    [Header("�ð�ȿ��")]
    [SerializeField]
    private float fadeInRate;
    public float FadeInRate { get { return fadeInRate; } set { fadeInRate = value; } }
    [SerializeField]
    private float fadeOutRate;
    public float FadeOutRate { get { return fadeOutRate; } set { fadeOutRate = value; } }
    [SerializeField]
    private float maxAlpha;
    public float MaxAlpha { get { return maxAlpha; } set { maxAlpha = value; } }

}

