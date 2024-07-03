using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    RangeDetecter detecter;
    public GameObject bulletPrefab;
    public List<GameObject> bullets;

    public float damage = 5;
    float fireRate = 3f;
    float timer = 0f;
    int fireCount = 3;

    void Start()
    {
        detecter = GetComponent<RangeDetecter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > fireRate)
        {
            Fire();
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void Fire()
    {
        var count = fireCount;

        foreach(GameObject bullet in bullets)
        {
            if(!bullet.activeSelf)
            {
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().damage = damage;
                count--;
            }
        }

        while(count > 0)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            if(bullet.GetComponent<Bullet>() != null)
            {
                bullet.GetComponent<Bullet>().damage = damage;
            }
            else
            {
                Debug.LogError("Bullet Prefab doesn't have script.");
            }
            bullets.Add(bullet);
            count--;
        }
    }


}
