using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class ItemDataJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ItemData);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        ItemData itemData = new ItemData();

        foreach (var property in jObject.Properties())
        {
            switch (property.Name)
            {
                case "Item_Id":
                    itemData.Item_Id = property.Value.ToObject<int>();
                    break;
                case "CurrentUpgrade":
                    itemData.CurrentUpgrade = property.Value.ToObject<int>();
                    break;
                
            }
        }

        return itemData;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        ItemData itemData = (ItemData)value;

        JObject jObject = new JObject();

        jObject.Add("Item_Id", itemData.Item_Id);
        jObject.Add("CurrentUpgrade", itemData.CurrentUpgrade);

        jObject.WriteTo(writer);
    }
}
