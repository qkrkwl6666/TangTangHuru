using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Magnet = 0,
    Experience,
    Heart,
    ReinforcedStone,
    EquipmentGemstone,
}

public interface IInGameItem
{
    public int ItemId {  get; set; }
    public string Name { get; set; }
    public ItemType ItemType { get; set; }
    public string TextureId { get; set; } 
    public void UseItem();
    public void GetItem();
}
