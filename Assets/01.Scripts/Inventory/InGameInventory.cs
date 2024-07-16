using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


public class InGameInventory : MonoBehaviour
{
    private List<IInGameItem> items = new ();
    private List<Image> images = new ();

    private void Awake()
    {
        foreach (Transform image in transform) 
        {
            var im = image.GetComponent<Image>();
            images.Add(im);
        }
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
            };

            break;
        }
    }

    public void RemoveItem(IInGameItem inGameItem)
    {
        items.Remove(inGameItem);
    }

}
