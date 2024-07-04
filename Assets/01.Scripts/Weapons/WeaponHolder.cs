using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    RangeDetecter detecter; //가장 가까운 적 위치감지

    public List<GameObject> WeaponPrefabs = new();
    public List<GameObject> bullets;

    public float damage = 5;
    public float bulletSpeed = 15f;

    float fireRate = 1.5f;
    float timer = 0f;
    int fireCount = 1;

    void Start()
    {
        detecter = GetComponent<RangeDetecter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > fireRate)
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
        var targetPos = detecter.nearest.transform.position;
        Vector2 velocity = Vector2.zero;

        if (targetPos != null)
        {
            velocity = targetPos - transform.position;
        }
        else
        {
            velocity = transform.position;
        }


        velocity = velocity.normalized * bulletSpeed;


        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {
                bullet.gameObject.transform.position = transform.position;
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.GetComponent<Rigidbody2D>().velocity = velocity;
                count--;
            }
        }

        while (count > 0)
        {
            var bullet = Instantiate(WeaponPrefabs[0], transform.position, Quaternion.identity);
            if (bullet.GetComponent<Bullet>() != null)
            {
                bullet.gameObject.transform.position = transform.position;
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.GetComponent<Rigidbody2D>().velocity = velocity;
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
