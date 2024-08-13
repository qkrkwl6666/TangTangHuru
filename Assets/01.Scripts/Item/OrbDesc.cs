using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class OrbDesc : MonoBehaviour
{
    public int orbId;
    public Image iconImage;
    public TextMeshProUGUI descripton;
    public Button button;

    private ItemData itemData;
    private ItemSlotUI connectedSlot;

    private void Start()
    {
    }

    public void SetInfo(int id)
    {
        orbId = id;
        itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(orbId.ToString());
        var stringDesc = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(itemData.Desc_Id);
        var stringType = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(itemData.Name_Id);

        descripton.text = stringDesc.Text + " / " + stringType.Text;
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (x) =>
        {
            iconImage.sprite = x.Result;
        };
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
