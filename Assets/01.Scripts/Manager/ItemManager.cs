using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public LinkedList<IInGameItem> inGameItems;


    public void AddItem(IInGameItem item)
    {
        if (item == null) return;


        inGameItems.AddFirst(item);
    }
}
