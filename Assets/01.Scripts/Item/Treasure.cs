using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IInGameItem
{
    // Todo : GameObejct 로 수정 
    public List<GameObject> inGameItems = new();

    public GameObject guardian;
    public int ItemId { get; set; }
    public string Name { get; set; }
    public IItemType ItemType { get; set; }
    public string TextureId { get; set; }

    public void AddItem(GameObject item)
    {
        item.SetActive(false);
        inGameItems.Add(item);
    }

    public void GetItem()
    {

    }

    public void UseItem()
    {
        // 상자 오픈 후 상자 자리에 아이템 스폰
        foreach (var item in inGameItems)
        {
            var dir = Random.insideUnitCircle;
            dir = dir.normalized * 3f;
            item.transform.position = (Vector2)transform.position + dir;
            item.SetActive(true);
        }

        if (guardian != null)
        {
            var dir = Random.insideUnitCircle;
            dir = dir.normalized * 10f;

            guardian.transform.position = (Vector2)transform.position + dir;
            guardian.SetActive(true);
        }

        // 상자 열고 난뒤
        gameObject.SetActive(false);
    }
}
