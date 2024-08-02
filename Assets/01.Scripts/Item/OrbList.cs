using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbList : MonoBehaviour
{
    public OrbDesc prefab_OrbDesc;
    public Image Icon_Orb;
    public Image Icon_OrbRare;
    public Image Icon_OrbEpic;

    public GameObject content;
    public List<OrbDesc> orbList = new();


    private void OnEnable()
    {
        int orbNum = GameManager.Instance.currSaveData.orb_Normal;
        int orbRareNum = GameManager.Instance.currSaveData.orb_Rare;
        int orbEpicNum = GameManager.Instance.currSaveData.orb_Epic;

        int draggableNum = orbNum + orbRareNum + orbEpicNum;

        for (int i = 0; i < draggableNum; i++)
        {
            orbList.Add(prefab_OrbDesc);

            if (i < orbNum)
            {
                orbList[i].iconImage = Icon_Orb;
            }
            else if (i >= orbNum && i < orbRareNum + orbNum)
            {
                orbList[i].iconImage = Icon_OrbRare;
            }
            else
            {
                orbList[i].iconImage = Icon_OrbEpic;
            }

            GameObject orb = Instantiate(orbList[i].gameObject);
            orb.transform.SetParent(content.transform);

        }

    }
}
