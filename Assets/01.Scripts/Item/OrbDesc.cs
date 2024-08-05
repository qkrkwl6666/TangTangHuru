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

    public void SetInfo(int id)
    {
        orbId = id;
        orbData = DataTableManager.Instance.Get<OrbTable>(DataTableManager.orb).GetOrbData(orbId.ToString());
        var stringDesc = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(orbData.Orb_Desc);
        var stringType = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(orbData.Orb_Type);

        descripton.text = stringDesc.Text + " / " + stringType.Text;
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
