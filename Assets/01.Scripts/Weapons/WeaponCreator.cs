using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static WeaponData;


public class DataInStage
{
    public int currWeaponLevel;
    public float damage;
    public float speed;
    public float range;
    public float coolDown;
    public int burstCount;
    public float burstRate;
    public int pierceCount;
    public float lifeTime;
    public float criticalChance;
    public float criticalValue;
}

public class WeaponCreator : MonoBehaviour
{
    //쿨타임마다, 연속발사 개수만큼 무기를 생성'만' 한다.
    //생성한 무기(총알, 화살 등의 발사체, 근접무기)의 오브젝트 풀을 갖고 있다.

    public GameObject weaponPrefab;
    public WeaponData weaponDataRef; //무기 원본 데이터
    
    //private WeaponData weaponDataInStage; //강화등에 의해 실시간 변경되는 스테이지 내 데이터

    private List<GameObject> weapons = new List<GameObject>();
    private IAimer aimer;
    private Hit hit;

    private WeaponUpgrader weaponUpgrader;

    private IEnumerator SpawnCoroutine;

    public DataInStage dataInStage;

    public bool levelUpReady = false;


    private void Awake()
    {
        dataInStage = new DataInStage();

        dataInStage.currWeaponLevel = 1;
        dataInStage.damage = weaponDataRef.Damage;
        dataInStage.speed = weaponDataRef.Speed;
        dataInStage.range = weaponDataRef.Range;
        dataInStage.coolDown = weaponDataRef.CoolDown;
        dataInStage.burstCount = weaponDataRef.BurstCount;
        dataInStage.burstRate = weaponDataRef.BurstRate;
        dataInStage.pierceCount = weaponDataRef.PierceCount;
        dataInStage.lifeTime = weaponDataRef.LifeTime;
        dataInStage.criticalChance = weaponDataRef.CriticalChance;
        dataInStage.criticalValue = weaponDataRef.CriticalValue;
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
            var count = 1;

            if (levelUpReady)
            {
                LevelUp();
                levelUpReady = false;
            }

            foreach (var weapon in weapons)
            {
                if (!weapon.activeSelf)
                {
                    weapon.gameObject.transform.position = transform.position;
                    weapon.SetActive(true);
                    weapon.GetComponent<IAimer>().Count = count;
                    count++;


                    if (dataInStage.burstRate > 0f)
                    {
                        yield return new WaitForSeconds(dataInStage.burstRate);
                    }
                }

                if (count > dataInStage.burstCount + 1)
                    break;
            }

            while (count < dataInStage.burstCount + 1)
            {
                CreateWeapon(count);
                count++;
                if (dataInStage.burstRate > 0f)
                {
                    yield return new WaitForSeconds(dataInStage.burstRate);
                }
            }

            yield return new WaitForSeconds(dataInStage.coolDown);
        }
    }


    public void CreateWeapon(int count)
    {
        var weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.SetActive(false);

        switch (weaponDataRef.WeaponAimType)
        {
            case Aim.Auto:
                aimer = weapon.AddComponent<AutoAim>();
                break;
            case Aim.Fixed:
                aimer = weapon.AddComponent<FixedAim>();
                break;
            case Aim.Manual:
                aimer = weapon.AddComponent<ManualAim>();
                break;
            case Aim.Player:
                aimer = weapon.AddComponent<PlayerAim>();
                break;
        }


        switch (weaponDataRef.WeaponAttackType)
        {
            case Attack.Melee:
                var melee = weapon.AddComponent<Melee>();
                melee.range = dataInStage.range;
                break;
            case Attack.Shoot:
                weapon.AddComponent<Shoot>();
                break;
            case Attack.Rotate:
                var rotate = weapon.AddComponent<Rotate>();
                rotate.angle = (360f / dataInStage.burstCount) * count;
                break;
        }

        aimer.LifeTime = dataInStage.lifeTime;
        aimer.Speed = dataInStage.speed;
        aimer.Count = count;

        hit = weapon.AddComponent<Hit>();
        hit.damage = dataInStage.damage;
        hit.pierceCount = dataInStage.pierceCount;
        hit.criticalChance = dataInStage.criticalChance;
        hit.criticalValue = dataInStage.criticalValue;
        hit.attackableLayer = LayerMask.GetMask("Enemy");

        weapon.transform.localScale = new Vector3 (dataInStage.range, dataInStage.range);

        weapon.SetActive(true);
        weapons.Add(weapon);
    }



    public void LevelUpReady()
    {
        levelUpReady = true;
    }

    public void LevelUp()
    {
        dataInStage = weaponUpgrader.UpgradeWeaponData(dataInStage);

        int count = 1;

        foreach (var weapon in weapons)
        {
            weapon.transform.localScale = new Vector3(dataInStage.range, dataInStage.range);
            aimer = weapon.GetComponent<IAimer>();
            aimer.LifeTime = dataInStage.lifeTime;
            aimer.Speed = dataInStage.speed;

            hit = weapon.GetComponent<Hit>();
            hit.damage = dataInStage.damage;
            hit.pierceCount = dataInStage.pierceCount;
            hit.criticalChance = dataInStage.criticalChance;
            hit.criticalValue = dataInStage.criticalValue;

            switch (weaponDataRef.WeaponAttackType)
            {
                case Attack.Melee:
                    break;
                case Attack.Shoot:
                    break;
                case Attack.Rotate:
                    weapon.GetComponent<Rotate>().angle = (360f / dataInStage.burstCount) * count;
                    count++;
                    break;
            }
        }
    }

}
