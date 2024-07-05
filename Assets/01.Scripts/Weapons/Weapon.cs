using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    RangeDetecter detecter;
    public GameObject bulletPrefab;
    public List<GameObject> bullets;

    Vector3 targetPos = Vector3.zero;

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

        //if (detecter.nearest != null)
        //{
        //    targetPos = detecter.nearest.transform.position;
        //}

        Vector2 velocity = Vector2.zero;

        velocity = targetPos - transform.position;
        velocity = velocity.normalized * bulletSpeed;

        foreach (GameObject bullet in bullets)
        {
            if(!bullet.activeSelf)
            {
                bullet.gameObject.transform.position = transform.position;
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.GetComponent<Rigidbody2D>().velocity = velocity;
                count--;
            }
        }

        while(count > 0)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            if(bullet.GetComponent<Bullet>() != null)
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
