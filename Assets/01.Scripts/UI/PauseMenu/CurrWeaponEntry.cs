using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrWeaponEntry : MonoBehaviour
{
    public Sprite powerTexture;
    public Sprite speedTexture;

    public Image typeIcon;
    public Image icon;
    public List<Image> levelStars;


    public void SetInfo(Sprite iconSprite, int level)
    {
        for(int i = 0;  i < levelStars.Count; i++)
        {
            levelStars[i].gameObject.SetActive(false);
        }

        icon.sprite = iconSprite;

        for (int i = 0; i < level; i++)
        {
            levelStars[i].gameObject.SetActive(true);
        }
    }

    public void SetType(WeaponData.Type type)
    {
        switch(type)
        {
            case WeaponData.Type.PowerType:
                typeIcon.sprite = powerTexture;
                typeIcon.gameObject.SetActive(true);
                break;
            case WeaponData.Type.SpeedType:
                typeIcon.sprite = speedTexture;
                typeIcon.gameObject.SetActive(true);
                break;

        }
    }
}
