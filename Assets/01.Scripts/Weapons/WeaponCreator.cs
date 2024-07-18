using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponData;

public class WeaponCreator : MonoBehaviour
{
    //쿨타임마다, 연속발사 개수만큼 무기를 생성'만' 한다.
    //생성한 무기(총알, 화살 등의 발사체, 근접무기)의 오브젝트 풀을 갖고 있다.

    public GameObject weaponPrefab;
    public WeaponData weaponDataRef; //무기 원본 데이터

    private WeaponData weaponDataInStage; //강화등에 의해 실시간 변경되는 스테이지 내 데이터

    private List<GameObject> weapons = new List<GameObject>();
    private IAimer aimer;
    private IAttackable hit;

    private WeaponUpgrader weaponUpgrader;

    private IEnumerator SpawnCoroutine;

    public bool levelUpReady = false;

    private void Awake()
    {
        weaponDataInStage = Instantiate(weaponDataRef);
    }

    private void Start()
    {
        weaponUpgrader = GetComponent<WeaponUpgrader>();
    }

    private void OnEnable()
    {
        SpawnCoroutine = Spawn();
        StartCoroutine(SpawnCoroutine);
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnCoroutine);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1.5f);

        while (gameObject.activeSelf)
        {
            var index = 0;

            foreach (var weapon in weapons)
            {
                if (!weapon.activeSelf)
                {
                    weapon.gameObject.transform.position = transform.position;
                    weapon.SetActive(true);
                    var aimer = weapon.GetComponent<IAimer>();
                    aimer.TotalCount = weaponDataInStage.BurstCount;
                    aimer.Index = index;

                    index++;
                }

                Options(weapon);

                if (index >= weaponDataInStage.BurstCount)
                    break;

                if (weaponDataInStage.BurstRate > 0f)
                {
                    yield return new WaitForSeconds(weaponDataInStage.BurstRate);
                }
            }

            while (index < weaponDataInStage.BurstCount)
            {
                CreateWeapon(index);
                index++;
                if (weaponDataInStage.BurstRate > 0f)
                {
                    yield return new WaitForSeconds(weaponDataInStage.BurstRate);
                }
            }

            if (levelUpReady)
            {
                LevelUp();
            }
            yield return new WaitForSeconds(weaponDataInStage.CoolDown);
        }
    }


    public void CreateWeapon(int count)
    {
        var weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.SetActive(false);

        switch (weaponDataRef.WeaponAimType)
        {
            case AimType.Auto:
                aimer = weapon.AddComponent<AutoAim>();
                break;
            case AimType.Fixed:
                aimer = weapon.AddComponent<FixedAim>();
                break;
            case AimType.Manual:
                aimer = weapon.AddComponent<ManualAim>();
                break;
            case AimType.Player:
                aimer = weapon.AddComponent<PlayerAim>();
                break;
        }


        switch (weaponDataRef.WeaponMoveType)
        {
            case MoveType.Melee:
                var melee = weapon.AddComponent<Melee>();
                melee.range = weaponDataInStage.Range;
                break;
            case MoveType.Shoot:
                weapon.AddComponent<Shoot>();
                break;
            case MoveType.WaveShoot:
                weapon.AddComponent<WaveShoot>();
                break;
            case MoveType.Rotate:
                var rotate = weapon.AddComponent<Rotate>();
                break;
            case MoveType.Fixed:
                weapon.AddComponent<Fixed>();
                break;
            case MoveType.Spread:
                var spread = weapon.AddComponent<Spread>();
                break;
            case MoveType.Laser:
                weapon.AddComponent<LaserShoot>();
                break;
        }

        switch (weaponDataRef.WeaponAttckType)
        {
            case AttackType.Enter:
                hit = weapon.AddComponent<HitOnEnter>();
                break;
            case AttackType.Stay:
                hit = weapon.AddComponent<HitOnStay>();
                break;
            case AttackType.OneOff:
                hit = weapon.AddComponent<HitOneOff>();
                break;
        }

        aimer.LifeTime = weaponDataInStage.LifeTime;
        aimer.Speed = weaponDataInStage.Speed;
        aimer.TotalCount = weaponDataInStage.BurstCount;
        aimer.Index = count;

        hit.Damage = weaponDataInStage.Damage;
        hit.PierceCount = weaponDataInStage.PierceCount;
        hit.CriticalChance = weaponDataInStage.CriticalChance;
        hit.CriticalValue = weaponDataInStage.CriticalValue;
        hit.AttackRate = weaponDataInStage.BurstRate;
        hit.AttackableLayer = LayerMask.GetMask("Enemy");

        foreach (var option in weaponDataInStage.Options)
        {
            if (option == Option.FadeInOut)
            {
                var fadeInOut = weapon.AddComponent<WeaponFadeInOut>();
                fadeInOut.fadeInDuration = weaponDataInStage.FadeInRate;
                fadeInOut.fadeOutDuration = weaponDataInStage.FadeOutRate;
                fadeInOut.maxAlpha = weaponDataInStage.MaxAlpha;
            }
        }

        weapon.transform.localScale = new Vector3 (weaponDataInStage.Range, weaponDataInStage.Range);
        weapon.SetActive(true);
        weapons.Add(weapon);

        Options(weapon);
    }

    public void LevelUpReady()
    {
        weaponDataInStage.Level++;
        levelUpReady = true;
    }

    private void LevelUp()
    {
        if(weaponDataInStage.Level >= 5)
        {
            weaponDataInStage.Level = 5;
            weaponUpgrader.Evolution(weapons);
        }
        else
        {
            weaponDataInStage = weaponUpgrader.UpgradeWeaponData(weaponDataInStage);
        }

        foreach (var weapon in weapons)
        {
            weapon.transform.localScale = new Vector3(weaponDataInStage.Range, weaponDataInStage.Range);
            aimer = weapon.GetComponent<IAimer>();
            aimer.LifeTime = weaponDataInStage.LifeTime;
            aimer.Speed = weaponDataInStage.Speed;

            hit = weapon.GetComponent<IAttackable>();
            hit.Damage = weaponDataInStage.Damage;
            hit.PierceCount = weaponDataInStage.PierceCount;
            hit.CriticalChance = weaponDataInStage.CriticalChance;
            hit.CriticalValue = weaponDataInStage.CriticalValue;
        }

        levelUpReady = false;
    }
      
    private void Options(GameObject weapon)
    {
        foreach (var option in weaponDataInStage.Options)
        {
            if (option == Option.Randomizer)
            {
                weapon.transform.position += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
            }
        }
    }
}
