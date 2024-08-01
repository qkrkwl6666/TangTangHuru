using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    private Image slotImage;
    // Start is called before the first frame update
    void Start()
    {
        slotImage = GetComponent<Image>();
        var slot = slotImage.GetComponentInChildren<Draggable>();
        if(slot == null)
        {
            Empty();
        }
    }


    public void Highlighted()
    {
        slotImage.color = Color.yellow;
    }
    public void Filled()
    {
        slotImage.color = Color.white;
    }
    public void Empty()
    {
        slotImage.color = Color.gray;
    }


}
