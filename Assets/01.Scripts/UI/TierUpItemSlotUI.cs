using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TierUpItemSlotUI : MonoBehaviour
{
    public Item item;

    public Image itemIcon;
    public Image outline;
    public Image background;
    public GameObject foucs;

    public bool IsFocus { get; private set; } = false;

    public void SetItemData(Item item)
    {
        this.item = item;

        // UI 아이템 데이터 에 맞춰서 설정

        // 아이템 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // 테두리 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        background.color = Defines.GetColor(item.itemData.Outline);
    }

    public void OnTierUpItemSlotButton()
    {
        IsFocus = !IsFocus;

        foucs.SetActive(IsFocus);
    }

}
