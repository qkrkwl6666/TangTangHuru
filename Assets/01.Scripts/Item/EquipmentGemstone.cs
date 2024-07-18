using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class EquipmentGemstone : MonoBehaviour, IInGameItem
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ItemType ItemType { get; set; } = ItemType.EquipmentGemstone;
    public string TextureId { get; set; }

    private InGameInventory inventory;

    private void Awake()
    {
        inventory = GameObject.FindWithTag("InGameInventory").GetComponent<InGameInventory>();
    }

    public void Init(ItemData itemdata)
    {
        ItemId = itemdata.Item_Id;
        Name = DataTableManager.Instance.Get<StringTable>(DataTableManager.String)
            .Get(itemdata.Name_Id).Text;
        TextureId = itemdata.Texture_Id;
    }

    public void GetItem()
    {

    }

    public void UseItem()
    {
        // 인게임 인벤토리에 들어가야 함
        Debug.Log($"아이템 ID : {ItemId}, 이름 : {Name}");
        inventory.AddItem(this);
    }
}
