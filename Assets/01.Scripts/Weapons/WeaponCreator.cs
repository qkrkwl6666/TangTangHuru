using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponData;

public class WeaponCreator : MonoBehaviour
{
    //��Ÿ�Ӹ���, ���ӹ߻� ������ŭ ���⸦ ����'��' �Ѵ�.
    //������ ����(�Ѿ�, ȭ�� ���� �߻�ü, ��������)�� ������Ʈ Ǯ�� ���� �ִ�.

    public GameObject weaponPrefab;
    public WeaponData weaponDataRef; //���� ���� ������

    private WeaponData weaponDataInStage; //��ȭ� ���� �ǽð� ����Ǵ� �������� �� ������

    private List<GameObject> weapons = new List<GameObject>();
    private IAimer aimer;
    private Hit hit;

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
            var count = 0;

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
                }

                if (count > weaponDataInStage.BurstCount)
                    break;

                if (weaponDataInStage.BurstRate > 0f)
                {
                    yield return new WaitForSeconds(weaponDataInStage.BurstRate);
                }
            }

            while (count < weaponDataInStage.BurstCount)
            {
                CreateWeapon(count);
                count++;
                if (weaponDataInStage.BurstRate > 0f)
                {
                    yield return new WaitForSeconds(weaponDataInStage.BurstRate);
                }
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
                melee.range = weaponDataInStage.Range;
                break;
            case Attack.Shoot:
                weapon.AddComponent<Shoot>();
                break;
            case Attack.Rotate:
                var rotate = weapon.AddComponent<Rotate>();
                rotate.angle = (360f / weaponDataInStage.BurstCount) * count;
                break;
            case Attack.Fixed:
                weapon.AddComponent<Fixed>();
                break;
            case Attack.Spread:
                weapon.AddComponent<Spread>();
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

        weapon.transform.localScale = new Vector3 (weaponDataInStage.Range, weaponDataInStage.Range);

        weapon.SetActive(true);
        weapons.Add(weapon);
    }

    public void LevelUpReady()
    {
        weaponDataInStage.Level++;
        levelUpReady = true;
    }

    public void LevelUp()
    {
        if(weaponDataInStage.Level < 5)
        {
            weaponDataInStage = weaponUpgrader.UpgradeWeaponData(weaponDataInStage);
        }
        else
        {
            weaponUpgrader.Evolution(weapons);
        }

        int count = 0;
        foreach (var weapon in weapons)
        {
            weapon.transform.localScale = new Vector3(weaponDataInStage.Range, weaponDataInStage.Range);
            aimer = weapon.GetComponent<IAimer>();
            aimer.LifeTime = weaponDataInStage.LifeTime;
            aimer.Speed = weaponDataInStage.Speed;

            hit = weapon.GetComponent<Hit>();
            hit.damage = weaponDataInStage.Damage;
            hit.pierceCount = weaponDataInStage.PierceCount;
            hit.criticalChance = weaponDataInStage.CriticalChance;
            hit.criticalValue = weaponDataInStage.CriticalValue;

            switch (weaponDataRef.WeaponAttackType)
            {
                case Attack.Melee:
                    break;
                case Attack.Shoot:
                    break;
                case Attack.Rotate:
                    weapon.GetComponent<Rotate>().angle = (360f / weaponDataInStage.BurstCount) * count;
                    count++;
                    break;
            }
        }
    }

}
