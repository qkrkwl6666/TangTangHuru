using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_UISlot : MonoBehaviour
{
    public ItemData itemData;

    public TextMeshProUGUI itemCountText;
    public Image itemIcon;
    public Image Outline;
    public Image Background;

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;

        // UI ������ ������ �� ���缭 ����
    }

    public void ItemSlotButton()
    {
        // ���� ������ Ÿ�Կ� �´� UI �˾� ���� ���� ������ ������ ���� �˾�����
        // �ѱ��
    }
}
