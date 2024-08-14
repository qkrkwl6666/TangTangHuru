using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


public class InGameInventory : MonoBehaviour
{
    private List<IInGameItem> items = new();
    private List<Image> images = new();

    private InGameUI gameUI;
    public int Kill { get; private set; }

    public static Action<int> OnCoinAdd;
    public static Action OnKillAdd;

    public RectTransform bagTransform;
    public RectTransform canvasTransform;

    // 플레이어 코인 수
    public int Coin { get; private set; } = 0;

    private void Awake()
    {
        foreach (Transform image in transform)
        {
            var im = image.GetComponent<Image>();
            im.color = new Color(255f, 255f, 255f, 0f);
            images.Add(im);
        }

        OnCoinAdd += AddCoin;
        OnKillAdd += AddKill;

        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();

    }

    private void OnEnable()
    {
        Boss.OnDead += GameClear;
    }

    private void OnDisable()
    {
        Boss.OnDead -= GameClear;
    }

    private void OnDestroy()
    {
        OnCoinAdd = null;
        OnKillAdd = null;
    }

    public void GameClear()
    {
        SaveItem();

        gameUI.SetGameClearUI(Coin, Kill);
    }

    public List<IInGameItem> SaveItem(bool isEffect = false)
    {
        List<IInGameItem> tempItems = new List<IInGameItem>();

        if (items.Count == 0) return null;

        foreach (var item in items)
        {
            switch (item.ItemType)
            {
                case IItemType.ReinforcedStone:
                    GameManager.Instance.AddinGameItem(item);
                    break;

                case IItemType.EquipmentGemstone:
                    GameManager.Instance.AddinGameItem(item);
                    tempItems.Add(item);
                    break;
            }
        }

        if(isEffect) ItemBagEffect(items);

        items.Clear();

        return items;
    }

    public void ItemBagEffect(List<IInGameItem> items)
    {
        for (int i = 0; i < items.Count; i++) 
        {
            images[i].gameObject.SetActive(false);
            images[i].sprite = null;

            var prefabId = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item)
                .GetItemData(items[i].ItemId.ToString()).Prefab_Id;

            Addressables.InstantiateAsync(prefabId).Completed += 
                (gemStone) =>
                {

                };
            
        }
    }

    public void AddItem(IInGameItem inGameItem)
    {
        items.Add(inGameItem);

        if (inGameItem.ItemType == IItemType.ReinforcedStone) return;

        foreach (Image image in images)
        {
            if (image.sprite != null) continue;

            Addressables.LoadAssetAsync<Sprite>(inGameItem.TextureId).Completed += (x) =>
            {
                image.sprite = x.Result;
                image.color = new Color(255f, 255f, 255f, 1f);
            };

            break;
        }
    }

    private void AddCoin(int value)
    {
        Coin += value;
        gameUI.UpdateCoinValue(Coin);
    }

    private void AddKill()
    {
        Kill++;
    }

    public void RemoveItem(IInGameItem inGameItem)
    {
        items.Remove(inGameItem);
    }

}
