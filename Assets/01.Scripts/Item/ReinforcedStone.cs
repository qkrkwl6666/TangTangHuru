using UnityEngine;

// °­È­¼®
public class ReinforcedStone : MonoBehaviour, IInGameItem
{
    public int ItemId { get; set; } = 600006;
    public string Name { get; set; } = DataTableManager.Instance.Get<StringTable>
        (DataTableManager.String).Get("Item_Name_Normal_Re_Stone").Text;
    public IItemType ItemType { get; set; } = IItemType.ReinforcedStone;
    public string TextureId { get; set; }

    private InGameInventory inventory;

    private void Awake()
    {
        inventory = GameObject.FindWithTag("InGameInventory").GetComponent<InGameInventory>();
    }

    public void GetItem()
    {

    }

    public void UseItem()
    {
        inventory.AddItem(this);
        gameObject.SetActive(false);
    }
}
