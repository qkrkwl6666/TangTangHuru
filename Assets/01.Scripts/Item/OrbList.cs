using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbList : MonoBehaviour
{
    public Image Icon_Orb;
    public Image Icon_OrbRare;
    public Image Icon_OrbEpic;

    public GameObject content;
    public List<OrbDesc> orbList;

    public ItemSlotUI currSlot;

    private int activeNum = 0;

    private bool reset = true;
    private int lastIndex;

    private OrbData orbData;

    private void OnEnable()
    {
        if (!reset)
            return;

        for (int i = 0; i < orbList.Count; i++)
        {
            orbList[i].UnSelected();
            orbList[i].gameObject.SetActive(false);
        }

        int orbRareNum = GameManager.Instance.currSaveData.orb_Atk_Rare;
        int orbEpicNum = GameManager.Instance.currSaveData.orb_Atk_Epic;
        int orbLegendNum = GameManager.Instance.currSaveData.orb_Atk_Legend;

        activeNum = orbRareNum + orbEpicNum + orbLegendNum;

        for (int i = 0; i < activeNum; i++)
        {
            if (i < orbRareNum)
            {
                orbData = DataTableManager.Instance.Get<OrbTable>(DataTableManager.orb).GetOrbData("610001");
                orbList[i].iconImage.sprite = Icon_Orb.sprite;
                orbList[i].orbId = 1;
                orbList[i].descripton.text = "노멀 오브 / 보통 능력치";
            }
            else if (i >= orbRareNum && i < orbRareNum + orbEpicNum)
            {
                orbData = DataTableManager.Instance.Get<OrbTable>(DataTableManager.orb).GetOrbData("610001");
                orbList[i].iconImage.sprite = Icon_OrbRare.sprite;
                orbList[i].orbId = 2;
                orbList[i].descripton.text = "레어 오브 / 좋은 능력치";
            }
            else
            {
                orbData = DataTableManager.Instance.Get<OrbTable>(DataTableManager.orb).GetOrbData("610001");
                orbList[i].iconImage.sprite = Icon_OrbEpic.sprite;
                orbList[i].orbId = 3;
                orbList[i].descripton.text = "에픽 오브 / 엄청 좋은 능력치";
            }
            orbList[i].gameObject.SetActive(true);

            int currIndex = i;
            orbList[i].GetComponentInChildren<Button>().onClick.AddListener(() => SelectOrb(currIndex));
        }

        reset = false;
    }

    public void UndoSelect()
    {
        for (int i = 0; i < activeNum; i++)
        {

        }
    }

    private void SelectOrb(int index)
    {
        orbList[index].Seleted();
        orbList[index].Connect(currSlot);
        currSlot.SetId(orbList[index].orbId);
        gameObject.SetActive(false);
        
    }

    public void ResetOn()
    {
        reset = true;
    }
}
