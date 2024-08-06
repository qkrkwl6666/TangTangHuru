using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                case "Name_Id":
                    itemData.Name_Id = property.Value.ToObject<string>();
                    break;
                case "Prefab_Id":
                    itemData.Prefab_Id = property.Value.ToObject<string>();
                    break;
                case "Desc_Id":
                    itemData.Desc_Id = property.Value.ToObject<string>();
                    break;
                case "Texture_Id":
                    itemData.Texture_Id = property.Value.ToObject<string>();
                    break;
                case "Spine_Id":
                    itemData.Spine_Id = property.Value.ToObject<string>();
                    break;
                case "Outline":
                    itemData.Outline = property.Value.ToObject<string>();
                    break;
                case "Item_Type":
                    itemData.Item_Type = property.Value.ToObject<int>();
                    break;
                case "Item_Tier":
                    itemData.Item_Tier = property.Value.ToObject<int>();
                    break;
                case "Damage":
                    itemData.Damage = property.Value.ToObject<float>();
                    break;
                case "Defense":
                    itemData.Defense = property.Value.ToObject<float>();
                    break;
                case "Hp":
                    itemData.Hp = property.Value.ToObject<float>();
                    break;
                case "Dodge":
                    itemData.Dodge = property.Value.ToObject<float>();
                    break;
                case "Damagecal":
                    itemData.Damagecal = property.Value.ToObject<float>();
                    break;
                case "CoolDown":
                    itemData.CoolDown = property.Value.ToObject<float>();
                    break;
                case "Criticalper":
                    itemData.Criticalper = property.Value.ToObject<float>();
                    break;
                case "Criticaldam":
                    itemData.Criticaldam = property.Value.ToObject<float>();
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
        jObject.Add("Name_Id", itemData.Name_Id);
        jObject.Add("Prefab_Id", itemData.Prefab_Id);
        jObject.Add("Desc_Id", itemData.Desc_Id);
        jObject.Add("Texture_Id", itemData.Texture_Id);
        jObject.Add("Spine_Id", itemData.Spine_Id);
        jObject.Add("Outline", itemData.Outline);
        jObject.Add("Item_Type", itemData.Item_Type);
        jObject.Add("Item_Tier", itemData.Item_Tier);
        jObject.Add("Damage", itemData.Damage);
        jObject.Add("Defense", itemData.Defense);
        jObject.Add("Hp", itemData.Hp);
        jObject.Add("Dodge", itemData.Dodge);
        jObject.Add("Damagecal", itemData.Damagecal);
        jObject.Add("CoolDown", itemData.CoolDown);
        jObject.Add("Criticalper", itemData.Criticalper);
        jObject.Add("Criticaldam", itemData.Criticaldam);

        jObject.WriteTo(writer);
    }
}
