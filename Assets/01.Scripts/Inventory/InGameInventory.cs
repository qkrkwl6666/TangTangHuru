using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInventory : MonoBehaviour
{
    private List<IInGameItem> items = new ();

    public void AddItem(IInGameItem inGameItem)
    {
        items.Add(inGameItem);
    }

    // 메인 인벤토리에 아이템 넣기
    public void GetItem()
    {
    
    }
}
