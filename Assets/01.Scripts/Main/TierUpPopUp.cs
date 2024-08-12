using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TierUpPopUp : MonoBehaviour
{
    public TextMeshProUGUI weaponTitleText;

    public Slider currentTierUpSlider;
    public TextMeshProUGUI currentTierUpText;

    public Transform content;

    public void SetItemUI(Item item)
    {
        string name = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(item.itemData.Name_Id).Text;
        weaponTitleText.text = $"{name} +{item.itemData.CurrentUpgrade}";
    }
}
