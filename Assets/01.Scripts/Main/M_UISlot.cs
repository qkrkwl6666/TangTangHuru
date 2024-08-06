using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class M_UISlot : MonoBehaviour
{
    public ItemData itemData;

    public GameObject textGameobject;

    public TextMeshProUGUI itemCountText;
    public Image itemIcon;
    public Image Outline;
    public Image Background;

    private bool isConsumable = false;

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;

        // UI ������ ������ �� ���缭 ����

        // ������ ������
        Addressables.LoadAssetAsync<Sprite>(itemData.Texture_Id).Completed += (texture) => 
        {
            itemIcon.sprite = texture.Result;
        };

        // �׵θ� ������
        Addressables.LoadAssetAsync<Sprite>(itemData.Outline).Completed += (texture) =>
        {
            Outline.sprite = texture.Result;
        };

        // ��� ����
        switch(itemData.Outline)
        {
            case "Outline_Blue":
                Background.color = Defines.blueColor;
                break;
            case "Outline_Green":
                Background.color = Defines.greenColor;
                break;
            case "Outline_Orange":
                Background.color = Defines.orangeColor;
                break;
            case "Outline_Purple":
                Background.color = Defines.purpleColor;
                break;
            case "Outline_Red":
                Background.color = Defines.redColor;
                break;
            case "Outline_White":
                Background.color = Defines.whiteColor;
                break;
            case "Outline_Yellow":
                Background.color = Defines.yellowColor;
                break;
        }
    }

    public void SetItemDataConsumable(ItemData itemData, int itemCount)
    {
        SetItemData(itemData);

        // �Ҹ�ǰ ���� ���� �ؽ�Ʈ ������Ʈ Ȱ��ȭ ���ְ� ���� �־��ֱ�
        textGameobject.SetActive(true);
        itemCountText.text = itemCount.ToString();
        isConsumable = true;
    }

    public void ItemSlotButton()
    {
        // ���� ������ Ÿ�Կ� �´� UI �˾� ���� ���� ������ ������ ���� �˾�����
        // �ѱ�� ���⼭ isConsumable �� ���� �˾� ���� �ٸ��� ����


    }
}
