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

    private List<Item> atkOrbList;
    private List<Item> defOrbList;
    private List<Item> HpOrbList;
    private List<Item> dodgeOrbList;

    private void OnEnable()
    {
        if (!reset)
            return;

        for (int i = 0; i < orbList.Count; i++)
        {
            orbList[i].UnSelected();
            orbList[i].gameObject.SetActive(false);
        }

        //�̰ɷ� ���� �޾ƿͼ� ��� ����ϰ�, �κ��丮 ���հ�� AddItem�޼ҵ�� �߰��ϱ�
        //���� �����Ǿ�� ��. Ÿ���� �ΰ� (Ÿ��1-����, Ÿ��2-��� ���� ��������)
        var rareOrbList = upgrader.inventory.GetItemTypesTier(ItemType.Orb, ItemTier.Rare);
        if(rareOrbList != null)
        {
            //foreach(Item orb in rareOrbList)
            //{
            //    orb.itemData.Damage
            //}
            activeNum += rareOrbList.Count;
        }
        var epicOrbList = upgrader.inventory.GetItemTypesTier(ItemType.Orb, ItemTier.Unique);
        if (epicOrbList != null)
        {
            activeNum += epicOrbList.Count;
        }
        var uniqueOrbList = upgrader.inventory.GetItemTypesTier(ItemType.Orb, ItemTier.Epic);
        if (uniqueOrbList != null)
        {
            activeNum += uniqueOrbList.Count;
        }
        var LegendOrbList = upgrader.inventory.GetItemTypesTier(ItemType.Orb, ItemTier.Legendary);
        if (LegendOrbList != null)
        {
            activeNum += LegendOrbList.Count;
        }


        for (int i = 0; i < activeNum; i++)
        {
            if (i < rareOrbList.Count)
            {
                orbList[i].SetInfo(610001);
            }
            else if (i >= uniqueOrbList.Count && i < uniqueOrbList.Count + uniqueOrbList.Count)
            {
                orbList[i].SetInfo(610002);
            }
            else if( i > (activeNum - LegendOrbList.Count))
            {
                orbList[i].SetInfo(610003);
            }
            else
            {
                orbList[i].SetInfo(610004);
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
