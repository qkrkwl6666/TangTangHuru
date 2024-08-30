using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public WeaponOrbSlot weaponOrbSlot;

    //public int currItemId;
    public Item currItem;
    public Image slotIcon;

    public bool isSelected = false;
    public OrbDesc connected;

    public bool isWeaponSlot = false;

    public Button button;

    private void OnEnable()
    {
        if (!isWeaponSlot)
            return;

        if (weaponOrbSlot != null)
            return;

        weaponOrbSlot = GetComponentInParent<WeaponOrbSlot>();
    }

    private void OnDisable()
    {
        if (isWeaponSlot)
            return;

        ClearInfo();
    }

    public void SetOrbInfo(OrbDesc orbDesc)
    {
        if (connected == null)
        {
            connected = orbDesc;
            connected.Seleted();
            connected.Connect(this);
            //currItemId = connected.orbId;
        }
        currItem = orbDesc.GetCurrItem();

        var itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(currItem.ItemId.ToString());
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (x) =>
        {
            slotIcon.sprite = x.Result;
            slotIcon.gameObject.SetActive(true);
        };
        isSelected = true;

    }

    public void AddOrbInfo()
    {
        if (isWeaponSlot)
        {
            weaponOrbSlot.AddOrbToWeapon(this);
        }
    }

    public void ClearInfo()
    {
        if (connected != null)
        {
            if (isWeaponSlot)
            {
                weaponOrbSlot.RemoveOrbFromWeapon(this);
            }
            connected.Disconnect();
            connected.UnSelected();
            connected = null;
        }
        currItem = null;
        isSelected = false;
        slotIcon.gameObject.SetActive(false);
    }

    public void SetButton()
    {
        if (connected != null)
        {
            if (isWeaponSlot)
            {
                weaponOrbSlot.RemoveOrbFromWeapon(this);
            }
            connected.Disconnect();
            connected.UnSelected();
            connected = null;
        }
        currItem = null;
    }
}
