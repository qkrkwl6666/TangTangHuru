using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        weaponUpgrader = GetComponent<WeaponUpgrader>();
    }

    private void OnEnable()
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

            var count = 1;

            foreach (var weapon in weapons)
            {
                if (!weapon.activeSelf)
                {
                    weapon.gameObject.transform.position = transform.position;
                    weapon.SetActive(true);
                    weapon.GetComponent<IAimer>().Count = count;

                    count++;

                    if(weaponDataInStage.BurstRate > 0f)
                    {
                        yield return new WaitForSeconds(weaponDataInStage.BurstRate);
                    }
                }

                if (count > weaponDataInStage.BurstCount + 1)
                    break;
            }

            while (count < weaponDataInStage.BurstCount + 1)
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
            case Aim.Player:
                aimer = weapon.AddComponent<PlayerAim>();
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
                var script = weapon.AddComponent<Rotate>();
                script.angle = (360f / weaponDataInStage.BurstCount) * count;
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


}
