using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public int currItemId;
    public Image slotIcon;

    public bool isSelected = false;

    public OrbDesc connected;

    public void SetOrbInfo(OrbDesc orbDesc)
    {
        connected = orbDesc;
        connected.Seleted();
        var itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(connected.orbId.ToString());
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (x) =>
        {
            slotIcon.sprite = x.Result;
            slotIcon.gameObject.SetActive(true);
        };

        isSelected = true;
    }

    public void ClearInfo()
    {
        connected.UnSelected();
        connected = null;
        currItemId = 0;
        isSelected = false;
        slotIcon.gameObject.SetActive(false);

    }

}
