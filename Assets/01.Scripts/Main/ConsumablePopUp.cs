using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ConsumablePopUp : MonoBehaviour
{
    // ������ ������
    public Image itemImage;
    public Image bgImage;
    public Image outlineImage;

    // ������ ����
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI itemDescText;

    public Button comfirmButton;
    public Button cencelButton;

    public MainUI mainUI;

    private void Awake()
    {
        comfirmButton.onClick.AddListener(OnComfirmButton);
        cencelButton.onClick.AddListener(OnComfirmButton);
    }

    public void SetItemUI(Item item)
    {
        // ������ �̹���
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Texture_Id).Completed +=
            (texture) =>
            {
                itemImage.sprite = texture.Result;
            };

        // ������ �ƿ����̳�
        Addressables.LoadAssetAsync<Sprite>(item.itemData.Outline).Completed +=
             (texture) =>
             {
                 outlineImage.sprite = texture.Result;
             };

        // ������ ���

        bgImage.color = Defines.GetColor(item.itemData.Outline);

        titleText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Name_Id.ToString()).Text;

        itemDescText.text = DataTableManager.Instance.Get<StringTable>
            (DataTableManager.String).Get(item.itemData.Desc_Id.ToString()).Text;
    }

    public void OnComfirmButton()
    {
        mainUI.SetActiveConsumablePopUpUI(false);
    }

}
