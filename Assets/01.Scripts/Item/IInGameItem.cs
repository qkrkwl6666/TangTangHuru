public enum IItemType
{
    Magnet = 0,
    Experience,
    Heart,
    ReinforcedStone,
    EquipmentGemstone,
}

public interface IInGameItem
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public IItemType ItemType { get; set; }
    public string TextureId { get; set; }
    public void UseItem();
    public void GetItem();
}
