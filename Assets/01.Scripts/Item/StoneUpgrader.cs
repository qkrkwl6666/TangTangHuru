using UnityEngine;
using UnityEngine.EventSystems;

public class StoneUpgrader : MonoBehaviour, IDropHandler
{
    public GameObject currentItem;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Draggable draggableItem = eventData.pointerDrag.GetComponent<Draggable>();
            if (draggableItem != null)
            {
                // 현재 슬롯이 비어있는지 확인
                if (currentItem == null)
                {
                    currentItem = draggableItem.gameObject;
                    draggableItem.transform.SetParent(transform);
                    draggableItem.transform.position = transform.position;
                }
            }
        }
    }

}
