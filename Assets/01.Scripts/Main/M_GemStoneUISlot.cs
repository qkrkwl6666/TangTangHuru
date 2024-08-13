using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_GemStoneUISlot : MonoBehaviour
{
    public ItemData itemData;

    public UnityEngine.UI.Button gemStoneButton;
    public UnityEngine.UI.Button subButton;

    public TextMeshProUGUI itemTotalCountText;
    public TextMeshProUGUI itemCurrentCountText;

    public Image itemIcon;
    public Image outline;
    public Image background;
    public GameObject foucs;

    private EquipmentAppraisal equipmentAppraisal;

    private void Awake()
    {
        gemStoneButton.onClick.AddListener(OnGemStoneButton);
        subButton.onClick.AddListener(OnSubButton);
    }

    public void SetItemData(ItemData itemData, EquipmentAppraisal equipmentAppraisal)
    {
        this.itemData = itemData;
        this.equipmentAppraisal = equipmentAppraisal;

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

    public void RefreshCurrentCount(int itemCount, int maxCount) 
    {
        itemCurrentCountText.text = $"{itemCount} /";
        itemTotalCountText.text = $" {maxCount}";
    }

    public void OnGemStoneButton()
    {
        bool isAdd = equipmentAppraisal.AddGemStone((ItemTier)itemData.Item_Tier);

        if (isAdd)
        {
            RefreshCurrentCount(equipmentAppraisal
                .GetSelectGemStoneCount((ItemTier)itemData.Item_Tier), 
                equipmentAppraisal
                .GetMaxGemStoneCount((ItemTier)itemData.Item_Tier));

            subButton.gameObject.SetActive(true);
        }
    }

    public void OnSubButton()
    {
        bool isSub = equipmentAppraisal.SubGemStone((ItemTier)itemData.Item_Tier);

        if (isSub)
        {
            RefreshCurrentCount(equipmentAppraisal
                .GetSelectGemStoneCount((ItemTier)itemData.Item_Tier),
                equipmentAppraisal
                .GetMaxGemStoneCount((ItemTier)itemData.Item_Tier));

            int currentCount = equipmentAppraisal
                .GetSelectGemStoneCount((ItemTier)itemData.Item_Tier);

            if (currentCount <= 0) subButton.gameObject.SetActive(false);
        }
    }

    public void SubButtonActive(bool active)
    {
        subButton.gameObject.SetActive(active);
    }
}
