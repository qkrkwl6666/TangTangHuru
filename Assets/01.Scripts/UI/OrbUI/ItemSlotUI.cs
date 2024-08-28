using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public WeaponOrbSlot weaponOrbSlot;

    public int currItemId;
    public Image slotIcon;

    public bool isSelected = false;
    public OrbDesc connected;

    public bool isWeaponSlot = false;

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
        currItemId = orbDesc.orbId;

        var itemData = DataTableManager.Instance.Get<ItemTable>(DataTableManager.item).GetItemData(currItemId.ToString());
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (x) =>
        {
            slotIcon.sprite = x.Result;
            slotIcon.gameObject.SetActive(true);

            if (isWeaponSlot)
            {
                weaponOrbSlot.AddOrbToWeapon(this);
            }
        };
        isSelected = true;


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
        currItemId = 0;
        isSelected = false;
        slotIcon.gameObject.SetActive(false);
    }

}
