using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoneUpgrader : MonoBehaviour, IDropHandler
{
    public GameObject currentItem;
    public ItemSlotUI upgradeSlot;
    public Button upgradeButton;

    public ItemSlotUI[] slots;

    private void Start()
    {
        slots = GetComponentsInChildren<ItemSlotUI>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == upgradeSlot)
        {
            Draggable draggableItem = eventData.pointerDrag.GetComponent<Draggable>();
            if (draggableItem != null)
            {
                // ���� ������ ����ִ��� Ȯ��
                if (currentItem == null)
                {
                    currentItem = draggableItem.gameObject;
                    draggableItem.transform.SetParent(transform);
                    draggableItem.transform.position = transform.position;
                }
                else
                {

                }
            }
        }
    }

        public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == upgradeSlot)
        {
            
        }
    }

}
