using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OrbPanel : MonoBehaviour
{
    public OrbUpgrader upgrader;
    public GameObject content;
    public List<OrbDesc> orbList;

    public ItemSlotUI currSlot;

    private int activeNum = 0;

    private bool reset = true;

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
            orbList[i].GetComponentInChildren<Button>().onClick.AddListener(() => upgrader.SelectOrbInPanel(currIndex));
        }

        reset = false;
    }

    public void ResetOn()
    {
        reset = true;
    }
}
