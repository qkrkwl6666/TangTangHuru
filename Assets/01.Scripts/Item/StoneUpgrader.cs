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
                // ���� ������ ����ִ��� Ȯ��
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
