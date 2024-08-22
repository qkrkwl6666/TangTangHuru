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


    [Header("이름, 레벨, 타입")]
    [SerializeField]
    private string weaponName;
    public string WeaponName { get { return weaponName; } }

    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField]
    private Type weaponType;
    public Type WeaponType { get { return weaponType; } set { weaponType = value; } }

    [Header("조준 방식")]
    [SerializeField]
    private AimType aimType;
    public AimType WeaponAimType { get { return aimType; } set { aimType = value; } }

    [Header("공격 방식(생성 후 움직임)")]
    [SerializeField]
    private MoveType moveType;
    public MoveType WeaponMoveType { get { return moveType; } set { moveType = value; } }

    [Header("공격 방식(다단 히트 여부)")]
    [SerializeField]
    private AttackType attackType;
    public AttackType WeaponAttckType { get { return attackType; } set { attackType = value; } }

    [Header("(다단 히트의 경우) 공격 간격")]
    [SerializeField]
    private float singleAttackRate;
    public float SingleAttackRate { get { return singleAttackRate; } set { singleAttackRate = value; } }

    [Header("대미지 관련 수치")]
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }
    [SerializeField]
    private float criticalChance;
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [SerializeField]
    private float criticalValue;
    public float CriticalValue { get { return criticalValue; } set { criticalValue = value; } }

    [Header("근접 거리")]
    [SerializeField]
    private float range;
    public float Range { get { return range; } set { range = value; } }

    [Header("넉백 크기")]
    [SerializeField]
    private float impact;
    public float Impact { get { return impact; } set { impact = value; } }

    [Header("투사체 속도")]
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [Header("투사체 크기")]
    [SerializeField]
    private float size;
    public float Size { get { return size; } set { size = value; } }

    [Header("투사체 유지시간")]
    [SerializeField]
    private float lifeTime;
    public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }

    [Header("발사 대기시간")]
    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } set { coolDown = value; } }

    [Header("점사 공격 개수 및 간격")]
    [SerializeField]
    private int burstCount;
    public int BurstCount { get { return burstCount; } set { burstCount = value; } }

    [SerializeField]
    private float burstRate;
    public float BurstRate { get { return burstRate; } set { burstRate = value; } }

    [Header("관통 회수")]
    [SerializeField]
    private int pierceCount;
    public int PierceCount { get { return pierceCount; } set { pierceCount = value; } }

    [Header("투사체 부가옵션")]
    public Option[] Options;

    [Header("착탄 공격 옵션 물체 개수")]
    [SerializeField]
    private int secondAtkCount;
    public int SecondAtkCount { get { return secondAtkCount; } set { secondAtkCount = value; } }
    [Header("시각효과")]
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

