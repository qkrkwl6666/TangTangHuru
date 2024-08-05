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

        // UI 아이템 데이터 에 맞춰서 설정
    }

    public void ItemSlotButton()
    {
        // 현재 아이템 타입에 맞는 UI 팝업 띄우고 현재 아이템 데이터 정보 팝업으로
        // 넘기기
    }
}
