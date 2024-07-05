using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCreator : MonoBehaviour
{
    //쿨타임마다, 연속발사 개수만큼 무기를 생성'만' 한다.
    //생성한 무기(총알, 화살 등의 발사체, 근접무기)의 오브젝트 풀을 갖고 있다.

    public GameObject weaponPrefab;

    public WeaponData weaponData;

    private List<GameObject> weapons = new List<GameObject>();
    private float timer = 0f;

    private float coolDown;
    private float burstRate;


    private void Start()
    {

    }

    void Update()
    {
        if (timer > weaponData.CoolDown)
        {
            StartCoroutine(Fire());
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void CreateWeapon()
    {

    }


    IEnumerator Fire()
    {
        var count = weaponData.BurstCount;


        foreach (var bullet in weapons)
        {
            if (!bullet.activeSelf)
            {
                bullet.gameObject.transform.position = transform.position;
                bullet.SetActive(true);
                count--;
                yield return new WaitForSeconds(weaponData.BurstRate);
            }
        }

        while (count > 0)
        {
            var bullet = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(true);
            weapons.Add(bullet);
            count--;
            yield return new WaitForSeconds(weaponData.BurstRate);
        }
    }
}
