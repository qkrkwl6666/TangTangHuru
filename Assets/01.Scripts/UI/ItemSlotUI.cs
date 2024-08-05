using System.Collections;
using System.Collections.Generic;
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
        var orbData = DataTableManager.Instance.Get<OrbTable>(DataTableManager.orb).GetOrbData(connected.orbId.ToString());
        Addressables.LoadAssetAsync<Sprite>(orbData.Orb_Texture).Completed += (x) =>
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
