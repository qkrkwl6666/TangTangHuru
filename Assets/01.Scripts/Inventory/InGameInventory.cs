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

    // ���� �κ��丮�� ������ �ֱ�
    public void GetItem()
    {
    
    }
}
