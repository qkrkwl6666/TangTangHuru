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


        for(int i = 1; i <= 4; i++)
        {
            var invenOrbList = upgrader.inventory.GetItemTypesTier(ItemType.Orb, (ItemTier)i);
            if (invenOrbList != null)
            {
                foreach (Item orb in invenOrbList)
                {
                    if (orb.itemData.Damage > 0)
                    {
                        atkOrbList.Add(orb);
                    }
                    else if(orb.itemData.Defense > 0)
                    {
                        defOrbList.Add(orb);
                    }
                    else if (orb.itemData.Hp > 0)
                    {
                        HpOrbList.Add(orb);
                    }
                    else
                    {
                        dodgeOrbList.Add(orb);
                    }
                }
                activeNum += invenOrbList.Count;
            }
        }

        

        for (int i = 0; i < activeNum; i++)
        {
            //if (i < rareOrbList.Count)
            //{
            //    orbList[i].SetInfo(610001);
            //}
            //else if (i >= uniqueOrbList.Count && i < uniqueOrbList.Count + uniqueOrbList.Count)
            //{
            //    orbList[i].SetInfo(610002);
            //}
            //else if (i > (activeNum - LegendOrbList.Count))
            //{
            //    orbList[i].SetInfo(610003);
            //}
            //else
            //{
            //    orbList[i].SetInfo(610004);
            //}

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
