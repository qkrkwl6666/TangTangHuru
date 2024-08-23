using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    public Slider hpSlider;

    public void SetInfo()
    {
        hpSlider.maxValue = GetComponent<LivingEntity>().startingHealth;
        hpSlider.value = hpSlider.maxValue;
    }



}
