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

        //�κ��丮���� ���� �޾ƿͼ� ��� ����ϰ�, �κ��丮 ���հ�� AddItem�޼ҵ�� �߰�
        int count = 0;
        for (int i = 1; i <= 4; ++i)
        {
            //var tierOrbList = upgrader.inventory.GetItemTypesTier(ItemType.Orb, (ItemTier)i);
            //if (tierOrbList != null)
            //{
            //    foreach ( var currOrb in tierOrbList)
            //    {
            //        orbList[count].SetInfo(currOrb.ItemId);
            //        orbList[count].gameObject.SetActive(true);

            //        int index = count;
            //        orbList[count].GetComponentInChildren<Button>().onClick.AddListener(() => upgrader.SelectOrbInPanel(index));
            //        count++;
            //    }
            //}
        }
        reset = false;
    }

    public void ResetOn()
    {
        reset = true;
    }
}
