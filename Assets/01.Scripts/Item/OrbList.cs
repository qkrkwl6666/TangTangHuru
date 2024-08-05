
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class OrbList : MonoBehaviour
{
    public GameObject content;
    public List<OrbDesc> orbList;

    public ItemSlotUI currSlot;

    private int activeNum = 0;

    private bool reset = true;
    private string iconImageID;

    private OrbTable orbTable;

    private List<Image> orbIconList = new();

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
        int orbUniqueNum = GameManager.Instance.currSaveData.orb_Atk_Unique;

        activeNum = orbRareNum + orbEpicNum + orbUniqueNum;


        for (int i = 0; i < activeNum; i++)
        {
            if (i < orbRareNum)
            {
                orbList[i].SetInfo(610003);
            }
            else if (i >= orbRareNum && i < orbRareNum + orbEpicNum)
            {
                orbList[i].SetInfo(610002);
            }
            else
            {
                orbList[i].SetInfo(610001);
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
        currSlot.SetId(610001);
        gameObject.SetActive(false);
        
    }

    public void ResetOn()
    {
        reset = true;
    }
}
