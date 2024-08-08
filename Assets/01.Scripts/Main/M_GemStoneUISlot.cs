using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_GemStoneUISlot : MonoBehaviour
{
    public ItemData itemData;

    private UnityEngine.UI.Button gemStoneButton;

    public TextMeshProUGUI itemTotalCountText;
    public TextMeshProUGUI itemCurrentCountText;

    public Image itemIcon;
    public Image outline;
    public Image background;
    public GameObject foucs;

    private void Awake()
    {
        gemStoneButton = GetComponent<Button>();

        gemStoneButton.onClick.AddListener(OnGemStoneButton);
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;

        // 아이템 아이콘
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // 테두리 아이콘
        Addressables.LoadAssetAsync<Sprite>(itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        background.color = Defines.GetColor(itemData.Outline);
    }

    public void RefreshItemCount(int itemCount)
    {
        itemCurrentCountText.text = "0 /";
        itemTotalCountText.text = $" {itemCount}";
    }

    public void OnGemStoneButton()
    {

    }
}
