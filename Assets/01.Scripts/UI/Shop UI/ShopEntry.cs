using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI price;
    public Button purchaseButton;


    public void SetInfo(string iconId, string titleId, string descId, int price)
    {
        Addressables.LoadAssetAsync<Sprite>(iconId).Completed +=
            (texture) =>
            {
                icon.sprite = texture.Result;
            };
        var stringMgr = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);
        this.title.text = stringMgr.Get(titleId).Text;
        this.desc.text = stringMgr.Get(descId).Text;
        this.price.text = price.ToString();
    }
}
