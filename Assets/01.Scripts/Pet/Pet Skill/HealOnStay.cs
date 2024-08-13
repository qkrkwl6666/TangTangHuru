using UnityEngine;

public class HealOnStay : MonoBehaviour
{
    public float HealAmount { get; set; }
    public float HealRate { get; set; }

    private float timer = 0f;
    private bool healReady = true;


    private void Update()
    {
        if (timer > HealRate)
        {
            healReady = true;
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && healReady)
        {
            other.gameObject.GetComponent<IDamagable>().OnDamage(-HealAmount, 0);
            healReady = false;
        }
    }
}
