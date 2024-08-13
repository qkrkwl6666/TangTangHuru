using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TierUpItemSlotUI : MonoBehaviour
{
    public Item item;

    public Image itemIcon;
    public Image outline;
    public Image background;
    public GameObject foucs;

    private TierUpPopUp tierUpPopUp;

    public bool IsFocus { get; private set; } = false;

    public void SetItemData(Item item, TierUpPopUp tierUpPopUp)
    {
        this.item = item;
        this.tierUpPopUp = tierUpPopUp;

        // UI ������ ������ �� ���缭 ����

        // ������ ������
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // �׵θ� ������
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        background.color = Defines.GetColor(item.itemData.Outline);
    }

    public void OnTierUpItemSlotButton()
    {
        if(tierUpPopUp.OnSlotButton(item, !IsFocus))
        {
            IsFocus = !IsFocus;

            foucs.SetActive(IsFocus);
        }
    }

}
