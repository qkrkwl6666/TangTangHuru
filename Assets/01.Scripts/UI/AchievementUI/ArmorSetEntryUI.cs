using System.Collections.Generic;
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

    private List<ItemData> items;
    public Button rewardButton;

    public void SetImages(int index)
    {
        items = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item)
                    .GetItemsbySet(ItemTier.Normal, index);

        foreach (var item in items)
        {
            switch (item.Item_Type)
            {
                case (int)ItemType.Helmet:
                    Addressables.LoadAssetAsync<Sprite>(item.Texture_Id).Completed += (x) =>
                    {
                        armorHead.sprite = x.Result;
                    };
                    break;
                case (int)ItemType.Armor:
                    Addressables.LoadAssetAsync<Sprite>(item.Texture_Id).Completed += (x) =>
                    {
                        armorBody.sprite = x.Result;
                    };
                    break;
                case (int)ItemType.Shose:
                    Addressables.LoadAssetAsync<Sprite>(item.Texture_Id).Completed += (x) =>
                    {
                        armorShoes.sprite = x.Result;
                    };
                    break;
            }
        }
    }

    public void CheckProgress(int index)
    {
        var armorList = AchievementManager.Instance.GetArmorNameList();

        foreach (var item in items)
        {
            switch (item.Item_Type)
            {
                case (int)ItemType.Helmet:
                    armorHeadDark.gameObject.SetActive(!AchievementManager.Instance.Check(armorList[index - 1]));
                    break;
                case (int)ItemType.Armor:
                    armorBodyDark.gameObject.SetActive(!AchievementManager.Instance.Check(armorList[index]));
                    break;
                case (int)ItemType.Shose:
                    armorShoesDark.gameObject.SetActive(!AchievementManager.Instance.Check(armorList[index + 1]));
                    break;
            }

            if (!armorHeadDark.gameObject.activeSelf
                && !armorBodyDark.gameObject.activeSelf
                && !armorShoesDark.gameObject.activeSelf)
            {
                rewardButton.interactable = true;
            }
            else
            {
                rewardButton.interactable = false;
            }
        }
    }
}
