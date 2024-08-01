using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{

    public Image slotIcon;
    private Image slotFrame;

    private void Start()
    {
        slotFrame = GetComponent<Image>();
    }

    public void Highlighted()
    {
        slotIcon.color = Color.yellow;
    }
    public void Filled()
    {
        slotIcon.color = Color.white;
    }
    public void Empty()
    {
        slotIcon.color = Color.gray;
    }


}
