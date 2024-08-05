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

    private OrbData orbData;
    private ItemSlotUI connectedSlot;

    private void Start()
    {
    }

    public void SetInfo(int orbId)
    {
        orbData = DataTableManager.Instance.Get<OrbTable>(DataTableManager.orb).GetOrbData(orbId.ToString());
        descripton.text = orbData.Orb_Name;
        Addressables.LoadAssetAsync<Sprite>(orbData.Orb_Texture).Completed += (x) =>
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
