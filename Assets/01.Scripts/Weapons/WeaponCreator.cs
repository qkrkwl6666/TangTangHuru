using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static WeaponData;

public class WeaponCreator : MonoBehaviour
{
    //쿨타임마다, 연속발사 개수만큼 무기를 생성하거나 기존 무기를 활성화한다.
    //생성한 무기(총알, 화살 등의 발사체, 근접무기)의 오브젝트 풀을 갖고 있다.

    public GameObject weaponPrefab;
    public WeaponData weaponDataRef; //무기 원본 데이터
    public GameObject secondWeaponPrefab;

    private WeaponData weaponDataInStage; //강화에 의해 변경되는 스테이지 내 데이터
    private List<GameObject> weapons = new List<GameObject>();
    private WeaponUpgrader weaponUpgrader;

    private IAimer aimer;
    private IProjectile projectile;
    private IAttackable hit;

    private IEnumerator SpawnCoroutine;

    private bool levelUpReady = false;

    private PassiveData EmptyData;
    private PassiveData typePassive; //파워, 스피드 타입으로 구분되는 패시브. 패시브매니저가 구분해서 할당함.
    private PassiveData commonPassive;

    public int currLevel = 0;

    public bool isMainWeapon = false;
    private float mainDamage;
    private float mainCoolDown;
    private float mainCriticalChance;
    private float mainCriticalValue;
    private WeaponData.Type mainType;

    private void Start()
    {
        weaponUpgrader = GetComponent<WeaponUpgrader>();
        weaponDataInStage = Instantiate(weaponDataRef);

        Addressables.LoadAssetAsync<PassiveData>("EmptyPassiveData").Completed += (EmptyData) =>
        {
            typePassive = EmptyData.Result;
            commonPassive = EmptyData.Result;
        };
    }

    private void OnEnable()
    {
        currLevel = 1;
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

        if (isMainWeapon)
        {
            weaponDataInStage.CriticalChance = mainCriticalChance;
            weaponDataInStage.CriticalValue = mainCriticalValue;
            weaponDataInStage.CoolDown = mainCoolDown;
        }

        SetWeaponData();

        while (gameObject.activeSelf)
        {
            var index = 0;

            foreach (var weapon in weapons)
            {
                if (!weapon.activeSelf)
                {
                    var aimer = weapon.GetComponent<IAimer>();
                    aimer.TotalCount = weaponDataInStage.BurstCount;
                    aimer.Index = index;

                    //SetWeaponData();

                    weapon.gameObject.transform.position = transform.position;
                    weapon.SetActive(true);

                    index++;
                }

                OptionsOnEnable(weapon);

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
            var coolReduce = weaponDataInStage.CoolDown * typePassive.CoolDown;
            yield return new WaitForSeconds(weaponDataInStage.CoolDown - coolReduce);
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
            case AimType.RandomTarget:
                aimer = weapon.AddComponent<RandomTarget>();
                break;
            case AimType.RandomSeed:
                aimer = weapon.AddComponent<RandomSeed>();
                break;
            case AimType.Angular:
                aimer = weapon.AddComponent<AngularAim>();
                break;

        }

        switch (weaponDataRef.WeaponMoveType)
        {
            case MoveType.Melee:
                projectile = weapon.AddComponent<Melee>();
                break;
            case MoveType.Shoot:
                projectile = weapon.AddComponent<Shoot>();
                break;
            case MoveType.WaveShot:
                projectile = weapon.AddComponent<WaveShoot>();
                break;
            case MoveType.Rotate:
                projectile = weapon.AddComponent<Rotate>();
                break;
            case MoveType.Fixed:
                projectile = weapon.AddComponent<Fixed>();
                break;
            case MoveType.SpreadShot:
                projectile = weapon.AddComponent<Spread>();
                break;
            case MoveType.SpreadWall:
                projectile = weapon.AddComponent<SpreadWall>();
                break;
            case MoveType.Laser:
                projectile = weapon.AddComponent<LaserShoot>();
                break;
            case MoveType.Spawn:
                projectile = weapon.AddComponent<Spawn>();
                break;
            case MoveType.Parabola:
                projectile = weapon.AddComponent<ParabolaShoot>();
                break;
            case MoveType.BackandForward:
                projectile = weapon.AddComponent<ParabolaRotate>();
                weapon.GetComponent<ParabolaRotate>().angleOffset = 0f;
                break;
            case MoveType.ParabolaRotate:
                projectile = weapon.AddComponent<ParabolaRotate>();
                weapon.GetComponent<ParabolaRotate>().angleOffset = 30f;
                break;
            case MoveType.Reflecting:
                projectile = weapon.AddComponent<ReflectingShoot>();
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

        aimer.Index = count;
        weapons.Add(weapon);

        SetWeaponData();

        OptionsOnCreate(weapon);
        OptionsOnEnable(weapon);

        weapon.SetActive(true);
    }

    public void SetWeaponData()
    {
        foreach (var weapon in weapons)
        {
            aimer = weapon.GetComponent<IAimer>();
            aimer.LifeTime = weaponDataInStage.LifeTime;
            aimer.TotalCount = weaponDataInStage.BurstCount;

            projectile = weapon.GetComponent<IProjectile>();
            projectile.Range = weaponDataInStage.Range;
            projectile.Size = weaponDataInStage.Size;
            projectile.Speed = weaponDataInStage.Speed;

            hit = weapon.GetComponent<IAttackable>();
            hit.PierceCount = weaponDataInStage.PierceCount;
            hit.AttackRate = weaponDataInStage.SingleAttackRate;
            hit.Impact = weaponDataInStage.Impact;
            hit.AttackableLayer = LayerMask.GetMask("Enemy");

            hit.CriticalChance = weaponDataInStage.CriticalChance + commonPassive.CriticalChance;
            hit.CriticalValue = weaponDataInStage.CriticalValue + commonPassive.CriticalValue;

            if (weaponDataInStage.WeaponType == mainType)
            {
                hit.Damage = (mainDamage * weaponDataInStage.Damage) + typePassive.Damage;
            }
            else
            {
                hit.Damage = ((mainDamage * 0.6f) * weaponDataInStage.Damage) + typePassive.Damage;
            } 

        }
    }

    public void LevelUpReady()
    {
        weaponDataInStage.Level++;
        currLevel = weaponDataInStage.Level;
        levelUpReady = true;
    }

    private void LevelUp()
    {
        if (weaponDataInStage.Level >= 5)
        {
            weaponDataInStage.Level = 5;
            weaponUpgrader.Evolution(weapons);
        }
        else
        {
            weaponDataInStage = weaponUpgrader.UpgradeWeaponData(weaponDataInStage);
        }

        SetWeaponData();

        levelUpReady = false;
    }

    private void OptionsOnCreate(GameObject weapon) //생성시 한번만 적용되는 옵션
    {
        foreach (var option in weaponDataInStage.Options)
        {
            if (option == Option.FadeInOut)
            {
                var fadeInOut = weapon.AddComponent<WeaponFadeInOut>();
                fadeInOut.fadeInDuration = weaponDataInStage.FadeInRate;
                fadeInOut.fadeOutDuration = weaponDataInStage.FadeOutRate;
                fadeInOut.maxAlpha = weaponDataInStage.MaxAlpha;
            }

            if (option == Option.SecondAttack)
            {
                var enabler = weapon.AddComponent<EnableOnDest>();
                enabler.SecondWeapon = secondWeaponPrefab;
                enabler.maxCount = weaponDataInStage.SecondAtkCount;
            }
        }
    }

    private void OptionsOnEnable(GameObject weapon) //재활용 될때마다 적용되는 옵션
    {
        foreach (var option in weaponDataInStage.Options)
        {
            if (option == Option.Randomizer)
            {
                weapon.transform.position += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
            }
        }
    }

    public void SetPassive(PassiveData typePassive, PassiveData commonPassive)
    {
        this.typePassive = typePassive;
        this.commonPassive = commonPassive;
    }
    public void SetMainInfo(float dmg, float coolDown, float criChance, float criValue, Type type)
    {
        mainDamage = dmg;
        mainCoolDown = coolDown;
        mainCriticalChance = criChance;
        mainCriticalValue = criValue;
        mainType = type;

    }
    public float GetMainDamage()
    {
        return mainDamage;
    }
    public float GetMainCoolDown()
    {
        return mainCoolDown;
    }
    public float GetMainCriChance()
    {
        return mainCriticalChance;
    }
    public float GetMainCriValue()
    {
        return mainCriticalValue;
    }
    public Type GetMainType()
    {
        return mainType;
    }
}
