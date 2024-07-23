using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnDest : MonoBehaviour
{
    public GameObject SecondWeapon;
    public int maxCount;
    private List<GameObject> secondWeapons = new List<GameObject>();
    private IAimer aimer;
    private float timer;
    private bool fired = false;

    void Start()
    {
        aimer = GetComponent<IAimer>();
    }

    void Update()
    {
        if(fired)
            return;

        if (timer >= (aimer.LifeTime - 0.1f))
        {
            ShowSecondAttack();
            timer = 0;
            fired = true;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        timer = 0;
        fired = false;
    }

    private void ShowSecondAttack()
    {
        int count = 0;

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
                break;
            }
        }

        for (int i = 0; i < maxCount - count; i++)
        {
            Create();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void Create()
    {
        var newWeapon = Instantiate(SecondWeapon);
        newWeapon.transform.position = transform.position;
        secondWeapons.Add(newWeapon);
    }
}
