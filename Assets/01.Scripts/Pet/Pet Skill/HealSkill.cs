using System.Collections;
using UnityEngine;

public class HealSkill : MonoBehaviour
{
    public GameObject groundPrefab;
    public bool isEvolved = false;

    private PlayerHealth playerHp;
    private float maxHp;
    private GameObject healGround;

    private float healAmount;
    private float currHealth = 0;

    private bool healReady = true;

    void Start()
    {
        playerHp = GetComponentInParent<PlayerHealth>();
        maxHp = playerHp.startingHealth;
        currHealth = maxHp;
        healAmount = maxHp / 20;

        if(!isEvolved)
        {
            healGround = Instantiate(groundPrefab);
            healGround.transform.localScale *= 1.8f;

            var onStay = healGround.AddComponent<HealOnStay>();
            onStay.HealAmount = healAmount;
            onStay.HealRate = 0.5f;
            healGround.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!healReady)
            return;

        if (playerHp.health < (maxHp / 2))
        {
            if (isEvolved)
            {
                StartCoroutine(ActiveHealBuff());

                Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll
                    (playerHp.transform.position, 20, LayerMask.NameToLayer("Enemy"));
                foreach(Collider2D collider in nearbyObjects)
                {
                    var monsterHp = collider.GetComponent<Monster>();
                    if (monsterHp != null)
                    {
                        monsterHp.OnDamage(0, 5);
                    }
                }
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
            playerHp.OnDamage(-healAmount, 0);
            yield return new WaitForSeconds(0.3f);
        }

        healReady = false;
    }
}
