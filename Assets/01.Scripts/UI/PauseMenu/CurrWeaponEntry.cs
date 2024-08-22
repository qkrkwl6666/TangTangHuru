using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrWeaponEntry : MonoBehaviour
{
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
}
