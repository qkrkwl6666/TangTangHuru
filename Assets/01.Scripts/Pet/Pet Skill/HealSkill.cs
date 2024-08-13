using System.Collections;
using UnityEngine;

public class HealSkill : MonoBehaviour
{
    public GameObject groundPrefab;
    public bool isEvolved;

    private PlayerHealth playerHp;
    private float maxHp;
    private GameObject healGround;

    private float healAmount;
    private bool healReady = true;

    void Start()
    {
        playerHp = GetComponentInParent<PlayerHealth>();
        maxHp = playerHp.startingHealth;
        healAmount = maxHp / 20;

        if(!isEvolved)
        {
            healGround = Instantiate(groundPrefab);
            healGround.transform.localScale *= 1.5f;

            var onStay = healGround.AddComponent<HealOnStay>();
            onStay.HealAmount = healAmount;
            onStay.HealRate = 0.5f;
            healGround.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!healReady)
            return;

        if (playerHp.health < (maxHp / 4))
        {
            if (isEvolved)
            {
                StartCoroutine(ActiveHealBuff());
            }
            else
            {
                ActiveHealGround();
            }
        }

    }

    private void ActiveHealGround()
    {
        healGround.transform.position = transform.position;
        healGround.SetActive(true);
        healReady = false;
    }


    IEnumerator ActiveHealBuff()
    {
        yield return null;

        for (int i = 0; i < 5; i++)
        {
            playerHp.OnDamage(healAmount, 0);
            yield return new WaitForSeconds(0.5f);
        }

        healReady = false;
    }
}
