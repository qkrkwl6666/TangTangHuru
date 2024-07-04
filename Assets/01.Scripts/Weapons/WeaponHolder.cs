using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    //��Ÿ�Ӹ���, ���ӹ߻� ������ŭ ���⸦ ����'��' �Ѵ�.
    //������ ����(�Ѿ�, ȭ�� ���� �߻�ü, ��������)�� ������Ʈ Ǯ�� ���� �ִ�.

    public List<GameObject> Weapons = new();
    private float timer = 0f;

    private WeaponInfo weaponInfo;

    private void Start()
    {
        weaponInfo = GetComponent<WeaponInfo>();
    }

    void Update()
    {
        if (timer > weaponInfo.weapon_CoolDown)
        {
            Fire();
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

        foreach (GameObject weapon in Weapons)
        {
            if (!weapon.activeSelf)
            {
                weapon.gameObject.transform.position = transform.position;
                weapon.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                count--;
            }
        }

        while (count > 0)
        {
            var weapon = Instantiate(Weapons[0], transform.position, Quaternion.identity);
            Weapons.Add(weapon);
            yield return new WaitForSeconds(0.3f);
            count--;
        }
    }
}
