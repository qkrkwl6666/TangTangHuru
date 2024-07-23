using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider expSlider;

    PlayerExp playerExp;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerExp = player.GetComponent<PlayerExp>();
            UpdateMaxValue();
        }
        else
        {
            Debug.LogError("No Player Here!");
        }

    }

    private void FixedUpdate()
    {
        expSlider.value = playerExp.CurrExp;
    }
    
    public void UpdateMaxValue()
    {
        expSlider.maxValue = playerExp.requiredExp;
        expSlider.value = playerExp.CurrExp;
    }


}
