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
        None,
    }
    public enum Attack
    {
        Melee,
        Shoot,
        Rotate,
    }

    [SerializeField]
    private string weaponName;
    public string WeaponName { get { return weaponName; } }

    [SerializeField]
    private Aim aimType;
    public Aim WeaponAimType { get { return aimType; } }
    [SerializeField]
    private Attack attackType;
    public Attack WeaponAttackType { get { return attackType; } }

    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } }

    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField]
    private float range;
    public float Range { get { return range; } }

    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } }

    [SerializeField]
    private float burstCount;
    public float BurstCount { get { return burstCount; } }
    [SerializeField]
    private float burstRate;
    public float BurstRate { get {  return burstRate; } }

    [SerializeField]
    private int pierceCount;
    public int PierceCount { get { return pierceCount; } }

    [SerializeField]
    private float lifeTime;
    public float LifeTime { get { return lifeTime; } }

    [SerializeField]
    private float criticalChance;
    public float CriticalChance { get { return criticalChance; } }

    [SerializeField]
    private float criticalValue;
    public float CriticalValue { get { return criticalValue; } }

    public int currWeaponLevel;

    public WeaponData DeepCopy()
    {
        WeaponData newCopy = CreateInstance<WeaponData>();
        newCopy.weaponName = weaponName;
        newCopy.aimType = aimType;
        newCopy.attackType = attackType;
        newCopy.damage = damage;
        newCopy.speed = speed;
        newCopy.range = range;
        newCopy.coolDown = coolDown;
        newCopy.burstCount = burstCount;
        newCopy.burstRate = burstRate;
        newCopy.pierceCount = pierceCount;
        newCopy.lifeTime = lifeTime;
        newCopy.criticalChance = criticalChance;
        newCopy.criticalValue = criticalValue;

        return newCopy;
    }

}

