using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private ItemSlotUI currSlot;

    public bool itemSelected;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        originalParent = transform.parent;
        var selectedSlot = originalParent.GetComponent<ItemSlotUI>();
        if (selectedSlot == null)
            return;

        if (itemSelected)
        {
            itemSelected = false;
        }
        else
        {
            itemSelected = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        currSlot = originalParent.GetComponent<ItemSlotUI>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetCanvas().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<ItemSlotUI>() == null)
        {
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
        }
        else
        {
            itemSelected = false;

            currSlot = eventData.pointerEnter.GetComponent<ItemSlotUI>();
            transform.SetParent(currSlot.transform);
            transform.position = currSlot.transform.position;
        }
    }

    private Canvas GetCanvas()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            return canvas;
        }
        else
        {
            Debug.LogError("Canvas가 없습니다!");
            return null;
        }
    }

}