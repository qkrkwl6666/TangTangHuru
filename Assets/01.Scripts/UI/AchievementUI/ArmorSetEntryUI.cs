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

    public void CheckProgress(int setIndex)
    {
        var armorList = AchievementManager.Instance.GetArmorNameList();

        // setIndex는 1~7 (세트 번호), armorList 인덱스는 (setIndex-1)*3 부터 시작
        int baseIndex = (setIndex - 1) * 3;

        foreach (var item in items)
        {
            switch (item.Item_Type)
            {
                case (int)ItemType.Helmet:
                    armorHeadDark.gameObject.SetActive(!AchievementManager.Instance.Check(armorList[baseIndex]));
                    break;
                case (int)ItemType.Armor:
                    armorBodyDark.gameObject.SetActive(!AchievementManager.Instance.Check(armorList[baseIndex + 1]));
                    break;
                case (int)ItemType.Shose:
                    armorShoesDark.gameObject.SetActive(!AchievementManager.Instance.Check(armorList[baseIndex + 2]));
                    break;
            }
        }

        // 모든 장비가 수집되었는지 확인 (foreach 밖에서 한 번만 체크)
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
