using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IInGameItem
{
    // Todo : GameObejct �� ���� 
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
        // ���� ���� �� ���� �ڸ��� ������ ����
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

        // ���� ���� ����
        gameObject.SetActive(false);
    }
}
