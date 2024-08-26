
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class IconLoader : MonoBehaviour
{
    private List<string> iconNames = new List<string>();
    private Dictionary<string, Sprite> iconDictionary = new Dictionary<string, Sprite>();

    void Awake()
    {
        iconNames.Add("IconSword");
        iconNames.Add("IconAxe");
        iconNames.Add("IconCrossbow");
        iconNames.Add("IconBow");
        iconNames.Add("IconStaff");
        iconNames.Add("IconWand");
        iconNames.Add("IconWave");
        iconNames.Add("IconNuckBack");
        iconNames.Add("IconBomb");
        iconNames.Add("IconLightning");
        iconNames.Add("IconReflect");
        iconNames.Add("IconFrozen");
        iconNames.Add("IconSpread");
        iconNames.Add("IconOrbit");
        iconNames.Add("IconPassive");
        iconNames.Add("IconEmptySlot");
    }
    private void Start()
    {
        foreach (string name in iconNames)
        {
            LoadIcon(name);
        }
    }

    private void LoadIcon(string iconName)
    {
        Addressables.LoadAssetAsync<Sprite>(iconName).Completed += handle => OnIconLoaded(handle, iconName);
    }

    private void OnIconLoaded(AsyncOperationHandle<Sprite> handle, string iconName)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            iconDictionary[iconName] = handle.Result; // 딕셔너리에 스프라이트 추가
        }
        else
        {
            Debug.LogError($"Failed to load {iconName} sprite.");
        }
    }

    public Sprite SetIconByName(string iconName)
    {
        if (iconDictionary.TryGetValue(iconName, out Sprite sprite))
        {
            return sprite; // 스프라이트를 Image 컴포넌트에 할당
        }
        else
        {
            Debug.LogError($"Icon with name {iconName} not found.");
            return null;
        }
    }
}
