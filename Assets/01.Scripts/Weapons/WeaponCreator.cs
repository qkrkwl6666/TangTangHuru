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
    //��Ÿ�Ӹ���, ���ӹ߻� ������ŭ ���⸦ ����'��' �Ѵ�.
    //������ ����(�Ѿ�, ȭ�� ���� �߻�ü, ��������)�� ������Ʈ Ǯ�� ���� �ִ�.

    public GameObject weaponPrefab;
    public WeaponData weaponDataRef; //���� ���� ������
    
    //private WeaponData weaponDataInStage; //��ȭ� ���� �ǽð� ����Ǵ� �������� �� ������

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
