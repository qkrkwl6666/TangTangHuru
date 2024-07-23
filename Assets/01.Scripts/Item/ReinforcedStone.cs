using UnityEngine;

// 강화석
public class ReinforcedStone : MonoBehaviour, IInGameItem
{
    public int ItemId { get; set; } = 600006;
    public string Name { get; set; } = DataTableManager.Instance.Get<StringTable>
        (DataTableManager.String).Get("Item_Name_Normal_Re_Stone").Text;
    public ItemType ItemType { get; set; } = ItemType.ReinforcedStone;
    public string TextureId { get ; set ; }

    public void GetItem()
    {
        
    }

    public void UseItem()
    {
        // Todo : 바로 메인 인벤토리 에 강화석이 들어가야함
        //Debug.Log($"아이템 ID : {ItemId}, 이름 : {Name}");
    }
}
