using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine.AddressableAssets;
using TextAsset = UnityEngine.TextAsset;

public class StringData
{
    public string String_Id { get; set; }
    public string Text { get; set; }
}

public class StringTable : DataTable
{
    public Dictionary<string, StringData> stringTable { get; private set; } = new();

    public StringData Get(string id)
    {
        if (!stringTable.ContainsKey(id)) return null;

        return stringTable[id];
    }

    public override void Load(string name, Action tableLoaded)
    {
        Addressables.LoadAssetAsync<TextAsset>(name).Completed += (textAsset) =>
        {
            using (var reader = new StringReader(textAsset.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<StringData>();

                foreach (var record in records)
                {
                    stringTable.Add(record.String_Id.ToString(), record);
                }
            }

            tableLoaded?.Invoke();
        };
    }

}
