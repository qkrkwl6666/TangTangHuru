using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnHit : MonoBehaviour
{
    public GameObject SecondWeapon;
    public int maxCount;
    LayerMask AttackableLayer;


    private int count = 0;
    private List<GameObject> secondWeapons = new List<GameObject>();

    void Start()
    {
        AttackableLayer = LayerMask.GetMask("Enemy");
        var hit = SecondWeapon.GetComponent<IAttackable>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((AttackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            foreach (var secondWeapon in secondWeapons)
            {
                if (!secondWeapon.activeSelf)
                {
                    secondWeapon.transform.position = transform.position;
                    secondWeapon.SetActive(true);
                    count++;
                }
                if (count >= maxCount)
                {
                    count = 0;
                    break;
                }
            }

            for (int i = 0; i < maxCount - count; i++)
            {
                Create();
            }
        }
    }

    private void Create()
    {
        var newWeapon = Instantiate(SecondWeapon);
        newWeapon.transform.position = transform.position;
        secondWeapons.Add(newWeapon);
    }
}
