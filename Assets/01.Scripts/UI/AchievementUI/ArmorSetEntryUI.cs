using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ArmorSetEntryUI : MonoBehaviour
{
    [SerializeField] private Image armorHead;
    [SerializeField] private Image armorBody;
    [SerializeField] private Image armorShoes;
    [SerializeField] private Image armorHeadDark;
    [SerializeField] private Image armorBodyDark;
    [SerializeField] private Image armorShoesDark;

    public Button rewardButton;

    public void SetImages(int index)
    {
        var items = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item)
                    .GetItemsbySet(ItemTier.Normal, index);

        foreach(var item in items)
        {
            switch(item.Item_Type)
            {
                case (int)ItemType.Helmet:
                    Addressables.LoadAssetAsync<Sprite>(item.Texture_Id).Completed += (x) =>
                    {
                        armorHead.sprite = x.Result;
                        if(CheckProgress(ItemType.Helmet, index))
                        {
                            armorHeadDark.gameObject.SetActive(false);
                        }
                        else
                        {
                            armorHeadDark.gameObject.SetActive(true);
                        }
                    };
                    break;
                case (int)ItemType.Armor:
                    Addressables.LoadAssetAsync<Sprite>(item.Texture_Id).Completed += (x) =>
                    {
                        armorBody.sprite = x.Result;
                        if (CheckProgress(ItemType.Armor, index))
                        {
                            armorBodyDark.gameObject.SetActive(false);
                        }
                        else
                        {
                            armorBodyDark.gameObject.SetActive(true);
                        }
                    };
                    break;
                case (int)ItemType.Shose:
                    Addressables.LoadAssetAsync<Sprite>(item.Texture_Id).Completed += (x) =>
                    {
                        armorShoes.sprite = x.Result;
                        if (CheckProgress(ItemType.Shose, index))
                        {
                            armorShoesDark.gameObject.SetActive(false);
                        }
                        else
                        {
                            armorShoesDark.gameObject.SetActive(true);
                        }
                    };
                    break;
            }
        }
    }

    private bool CheckProgress(ItemType type, int index)
    {
        var armorList = AchievementManager.Instance.myTasks.armorSetNames;

        switch (type)
        {
            case ItemType.Helmet:
                return AchievementManager.Instance.Check(armorList[index-1]);

            case ItemType.Armor:
                return AchievementManager.Instance.Check(armorList[index]);

            case ItemType.Shose:
                return AchievementManager.Instance.Check(armorList[index+1]);
            default:
                return false;
        }
    }
}
