using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public int currItemId;
    public Image slotIcon;

    public bool isSelected = false;
    public OrbDesc connected;

    private void OnDisable()
    {
        ClearInfo();
    }

    public void SetOrbInfo(OrbDesc orbDesc)
    {
        if (connected != null)
        {
            connected = orbDesc;
            connected.Seleted();
            //currItemId = connected.orbId;
        }
        currItemId = orbDesc.orbId;

        var itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(currItemId.ToString());
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (x) =>
        {
            slotIcon.sprite = x.Result;
            slotIcon.gameObject.SetActive(true);
        };
        isSelected = true;
    }

    public void ClearInfo()
    {
        if (connected != null)
        {
            connected.UnSelected();
            connected = null;
        }
        currItemId = 0;
        isSelected = false;
        slotIcon.gameObject.SetActive(false);
    }

}
