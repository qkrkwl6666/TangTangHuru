using UnityEngine;

public class EquipmentGemstone : MonoBehaviour, IInGameItem
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public IItemType ItemType { get; set; } = IItemType.EquipmentGemstone;
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
        inventory.AddItem(this);
        gameObject.SetActive(false);
    }
}
