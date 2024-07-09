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
    public WeaponData weaponData;
    IAimer aimer;
    Hit hit;

    private List<GameObject> weapons = new List<GameObject>();

    private IEnumerator SpawnCoroutine;

    private void Start()
    {
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
            yield return new WaitForSeconds(weaponData.CoolDown);

            var count = weaponData.BurstCount;

            foreach (var weapon in weapons)
            {
                if (!weapon.activeSelf)
                {
                    weapon.gameObject.transform.position = transform.position;
                    weapon.SetActive(true);
                    count--;
                    yield return new WaitForSeconds(weaponData.BurstRate);
                }

                if (count <= 0)
                    break;
            }

            while (count > 0)
            {
                CreateWeapon();
                count--;
                yield return new WaitForSeconds(weaponData.BurstRate);
            }
        }
    }


    public void CreateWeapon()
    {
        var weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.SetActive(false);

        switch (weaponData.WeaponAimType)
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


        switch (weaponData.WeaponAttackType)
        {
            case Attack.Melee:
                weapon.AddComponent<Melee>();
                break;
            case Attack.Shoot:
                weapon.AddComponent<Shoot>();
                break;
            case Attack.Rotate:
                weapon.AddComponent<Rotate>(); 
                break;
        }

        aimer.LifeTime = weaponData.LifeTime;
        aimer.Speed = weaponData.Speed;

        hit = weapon.AddComponent<Hit>();
        hit.damage = weaponData.Damage;
        hit.pierceCount = weaponData.PierceCount;
        hit.attackableLayer = LayerMask.GetMask("Enemy");

        weapon.transform.localScale *= weaponData.Range;

        weapon.SetActive(true);
        weapons.Add(weapon);
    }


    public void UpgradeWeapon()
    {
        foreach (var weapon in weapons)
        {
            var aimer = weapon.GetComponent<IAimer>();
            aimer.Speed = weaponData.Speed;
            aimer.LifeTime = weaponData.LifeTime;
            hit.damage = weaponData.Damage;
            hit.pierceCount = weaponData.PierceCount;
        }
    }
}
