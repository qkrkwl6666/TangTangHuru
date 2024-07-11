using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static WeaponData;

public class WeaponCreator : MonoBehaviour
{
    //쿨타임마다, 연속발사 개수만큼 무기를 생성'만' 한다.
    //생성한 무기(총알, 화살 등의 발사체, 근접무기)의 오브젝트 풀을 갖고 있다.

    public GameObject weaponPrefab;
    public WeaponData weaponDataRef; //무기 원본 데이터
    public SkillUpgradeData skillUpgradeData; //스테이지내 업그레이드 데이터

    private WeaponData weaponDataInStage; //강화등에 의해 실시간 변경되는 스테이지 내 데이터

    IAimer aimer;
    Hit hit;

    private List<GameObject> weapons = new List<GameObject>();

    private IEnumerator SpawnCoroutine;

    private void Start()
    {
        weaponDataInStage = weaponDataRef.DeepCopy();

        SpawnCoroutine = Spawn();
        StartCoroutine(SpawnCoroutine);
    }

    void Update() 
    {

    }

    private void OnDisable()
    {
        StopCoroutine(SpawnCoroutine);
    }

    IEnumerator Spawn()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(weaponDataInStage.CoolDown);

            var count = 1;

            foreach (var weapon in weapons)
            {
                if (!weapon.activeSelf)
                {
                    weapon.gameObject.transform.position = transform.position;
                    weapon.SetActive(true);
                    weapon.GetComponent<IAimer>().Count = count;

                    count++;

                    yield return new WaitForSeconds(weaponDataInStage.BurstRate);
                }

                if (count > weaponDataInStage.BurstCount + 1)
                    break;
            }

            while (count < weaponDataInStage.BurstCount + 1)
            {
                CreateWeapon(count);
                count++;
                yield return new WaitForSeconds(weaponDataInStage.BurstRate);
            }
        }
    }


    public void CreateWeapon(int count)
    {
        var weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.SetActive(false);

        switch (weaponDataInStage.WeaponAimType)
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
        }


        switch (weaponDataInStage.WeaponAttackType)
        {
            case Attack.Melee:
                weapon.AddComponent<Melee>();
                break;
            case Attack.Shoot:
                weapon.AddComponent<Shoot>();
                break;
            case Attack.Rotate:
                weapon.AddComponent<Rotate>();
                weapon.GetComponent<Rotate>().angle = (360f / weaponDataInStage.BurstCount) * count;
                break;
        }

        aimer.LifeTime = weaponDataInStage.LifeTime;
        aimer.Speed = weaponDataInStage.Speed;
        aimer.Count = count;

        hit = weapon.AddComponent<Hit>();
        hit.damage = weaponDataInStage.Damage;
        hit.pierceCount = weaponDataInStage.PierceCount;
        hit.criticalChance = weaponDataInStage.CriticalChance;
        hit.criticalValue = weaponDataInStage.CriticalValue;
        hit.attackableLayer = LayerMask.GetMask("Enemy");

        weapon.transform.localScale *= weaponDataInStage.Range;

        weapon.SetActive(true);
        weapons.Add(weapon);
    }


    public void UpgradeWeapon(int num)
    {
        //weaponDataInStage 을 업그레이드 계수에 맞춰 갱신하는 기능추가
        weaponDataInStage.currWeaponLevel++;

        foreach (var weapon in weapons)
        {
            var aimer = weapon.GetComponent<IAimer>();
            aimer.Speed = weaponDataInStage.Speed;
            aimer.LifeTime = weaponDataInStage.LifeTime;
            hit.damage = weaponDataInStage.Damage;
            hit.pierceCount = weaponDataInStage.PierceCount;
            hit.criticalChance = weaponDataInStage.CriticalChance;
            hit.criticalValue = weaponDataInStage.CriticalValue;
        }
    }
}
