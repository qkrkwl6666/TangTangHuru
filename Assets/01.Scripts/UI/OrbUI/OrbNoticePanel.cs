using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class OrbNoticePanel : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI noticeText;
    public Button cancelButton;

    public void Start()
    {
        cancelButton.onClick.AddListener(ClosePanel);
    }

    public void SetInfo(ItemData itemData)
    {
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (texture) =>
        {
            icon.sprite = texture.Result;
        };

        var orbName = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(itemData.Name_Id).Text;

        var orbDesc = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).Get(itemData.Desc_Id).Text;

        noticeText.text = $"{orbDesc} {orbName} È¹µæ!";
    }

    private void ClosePanel()
    {
        SoundManager.Instance.PlaySound2D("cancel");
        gameObject.SetActive(false);
    }
}
