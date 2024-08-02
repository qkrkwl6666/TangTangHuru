using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbDesc : MonoBehaviour
{
    public int orbId;
    public Image iconImage;
    public TextMeshProUGUI descripton;
    public Button button;

    private ItemSlotUI connectedSlot;

    private void Start()
    {
    }

    public void Connect(ItemSlotUI currSlot)
    {
        connectedSlot = currSlot;
    }

    public void Disconnect()
    {
        connectedSlot = null;
    }

    public void Seleted()
    {
        button.interactable = false;
    }

    public void UnSelected()
    {
        button.interactable = true;
    }
}
