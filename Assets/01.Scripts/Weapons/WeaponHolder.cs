using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    //쿨타임마다, 연속발사 개수만큼 무기를 생성'만' 한다.
    //생성한 무기(총알, 화살 등의 발사체, 근접무기)의 오브젝트 풀을 갖고 있다.

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
