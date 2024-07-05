using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    //��Ÿ�Ӹ���, ���ӹ߻� ������ŭ ���⸦ ����'��' �Ѵ�.
    //������ ����(�Ѿ�, ȭ�� ���� �߻�ü, ��������)�� ������Ʈ Ǯ�� ���� �ִ�.

    public GameObject weaponPrefab;
    private List<GameObject> weapons = new List<GameObject>();
    private float timer = 0f;

    private WeaponInfo weaponInfo;

    private float coolDown;
    private float burstRate;


    private void Start()
    {
        weaponInfo = GetComponent<WeaponInfo>();
    }

    void Update()
    {
        if (timer > weaponInfo.weapon_CoolDown)
        {
            StartCoroutine(Fire());
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

   IEnumerator Fire()
    {
        var count = weaponInfo.weapon_BurstCount;

        foreach (var bullet in weapons)
        {
            if (!bullet.activeSelf)
            {
                bullet.gameObject.transform.position = transform.position;
                bullet.SetActive(true);
                count--;
                yield return new WaitForSeconds(weaponInfo.weapon_BurstRate);
            }
        }

        while (count > 0)
        {
            var bullet = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(true);
            weapons.Add(bullet);
            count--;
            yield return new WaitForSeconds(weaponInfo.weapon_BurstRate);
        }
    }
}
