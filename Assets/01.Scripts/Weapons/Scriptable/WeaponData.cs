using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    public enum Aim
    {
        Auto,
        Manual,
        Fixed,
        Spawn,
        Player,
    }
    public enum Attack
    {
        Melee,
        Shoot,
        Rotate,
        Fixed,
    }

    [SerializeField]
    private string weaponName;
    public string WeaponName { get { return weaponName; } }

    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField]
    private Aim aimType;
    public Aim WeaponAimType { get { return aimType; } set { aimType = value; } }
    [SerializeField]
    private Attack attackType;
    public Attack WeaponAttackType { get { return attackType; } set { attackType = value; } }

    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [SerializeField]
    private float range;
    public float Range { get { return range; } set { range = value; } }

    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } set { coolDown = value; } }

    [SerializeField]
    private int burstCount;
    public int BurstCount { get { return burstCount; } set { burstCount = value; } }

    [SerializeField]
    private float burstRate;
    public float BurstRate { get {  return burstRate; } set { burstRate = value; } }

    [SerializeField]
    private int pierceCount;
    public int PierceCount { get { return pierceCount; } set { pierceCount = value; } }

    [SerializeField]
    private float lifeTime;
    public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }

    [SerializeField]
    private float criticalChance;
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [SerializeField]
    private float criticalValue;
    public float CriticalValue { get { return criticalValue; } set { criticalValue = value; } }
}

