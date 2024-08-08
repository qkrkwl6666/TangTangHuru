using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_GemStoneUISlot : MonoBehaviour
{
    public Item item;

    private UnityEngine.UI.Button gemStoneButton;

    public TextMeshProUGUI itemCountText;
    public Image itemIcon;
    public Image outline;
    public Image background;
    public GameObject foucs;

    private void Awake()
    {
        gemStoneButton = GetComponent<Button>();

        gemStoneButton.onClick.AddListener(OnGemStoneButton);
    }

    public void SetItemData(Item item)
    {
        this.item = item;

        // 아이템 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed += (texture) =>
        {
            itemIcon.sprite = texture.Result;
        };

        // 테두리 아이콘
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed += (texture) =>
        {
            outline.sprite = texture.Result;
        };

        background.color = Defines.GetColor(item.itemData.Outline);
    }

    public void RefreshItemCount(int itemCount)
    {
        itemCountText.text = itemCount.ToString();
    }

    public void OnGemStoneButton()
    {

    }
}
