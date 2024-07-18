using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;
using CsvHelper;
using System.Globalization;
using System.IO;

public class ItemData
{
    public int Item_Id { get; set; }
    public string Prefab_Id { get; set; }
    public string Name_Id { get; set; }
    public string Desc_Id { get; set; }
    public string Texture_Id { get; set; }
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
