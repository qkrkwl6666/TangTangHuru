using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


public class InGameInventory : MonoBehaviour
{
    private List<IInGameItem> items = new ();
    private List<Image> images = new ();

    private InGameUI gameUI;

    public static Action<int> OnCoinAdd;

    // 플레이어 코인 수
    public int Coin { get; private set; } = 0;

    private void Awake()
    {
        foreach (Transform image in transform) 
        {
            var im = image.GetComponent<Image>();
            im.color = new Color(255f, 255f,255f ,0f);
            images.Add(im);
        }

        OnCoinAdd += AddCoin;

        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
    }

    private void OnDestroy()
    {
        OnCoinAdd = null;
    }

    public void AddItem(IInGameItem inGameItem)
    {
        items.Add(inGameItem);

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

    public void RemoveItem(IInGameItem inGameItem)
    {
        items.Remove(inGameItem);
    }

}
