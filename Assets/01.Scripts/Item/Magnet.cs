using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, IInGameItem
{
    public int ItemId { get ; set ; }
    public string Name { get ; set ; }
    public IItemType ItemType { get ; set ; }
    public string TextureId { get ; set ; }

    public ItemDetection ItemDetection { get ; set ; }

    private void Awake()
    {
        ItemDetection = GameObject.FindWithTag("Player").GetComponentInChildren<ItemDetection>();
    }

    public void GetItem()
    {
        
    }

    public void UseItem()
    {
        ItemDetection.MagnetOn();
    }
}
