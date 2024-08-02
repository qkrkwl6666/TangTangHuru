using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class ItemData
{
    public int Item_Id { get; set; }
    public string Name_Id { get; set; }
    public string Prefab_Id { get; set; }
    public string Desc_Id { get; set; }
    public string Texture_Id { get; set; }
    public string Spine_Id { get; set; }
    public string Outline { get; set; }
    public int Item_Type { get; set; }
    public int Item_Tier { get; set; }
    public float Damage { get; set; }
    public float Defense { get; set; }
    public float Hp { get; set; }
    public float Dodge { get; set; }
    public float Damagecal { get; set; }
    public float CoolDown { get; set; }
    public float Criticalper { get; set; }
    public float Criticaldam { get; set; }
}

public class ItemTable : DataTable
{
    public Dictionary<string, ItemData> itemTable { get; private set; } = new();

    public ItemData GetItemData(string item_Id)
    {
        if (!itemTable.ContainsKey(item_Id)) return null;

        return itemTable[item_Id];
    }

    public override void Load(string name)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<ItemData>();

                foreach (var record in records)
                {
                    itemTable.Add(record.Item_Id.ToString(), record);
                }
            }
        };
    }

}
