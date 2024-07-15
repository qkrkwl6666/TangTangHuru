using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public List<IInGameItem> inGameItems = new ();

    public void AddItem(IInGameItem item)
    {
        inGameItems.Add(item);
    }

    public void OpenItem()
    {
        // 상자 오픈 후 상자 자리에 아이템 스폰
    }
}
