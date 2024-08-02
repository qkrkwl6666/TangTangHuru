using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public int currItemId;
    public Image slotIcon;
   

    private void Start()
    {

    }

    public void SetId(int id)
    {
        currItemId = id;

        string textureName = "IconOrbNormal";

        Addressables.LoadAssetAsync<Sprite>(textureName).Completed += (x) =>
        {
            slotIcon.sprite = x.Result;
            slotIcon.color = new Color(255f, 255f, 255f, 1f);
            slotIcon.gameObject.SetActive(true);
        };
    }

    public void Highlighted()
    {
        slotIcon.color = Color.yellow;
    }
    public void Filled()
    {
        slotIcon.color = Color.white;
    }
    public void Empty()
    {
        slotIcon.color = Color.gray;
    }


}
