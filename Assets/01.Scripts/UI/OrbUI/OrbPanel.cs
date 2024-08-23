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

    private List<ItemType> orbTypes = new List<ItemType>
    {
        ItemType.OrbAttack,
        ItemType.OrbDefence,
        ItemType.OrbHp,
        ItemType.OrbDodge,
    };

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
        List<Item> sortedOrbList = new();
        for (int i = 0; i < orbTypes.Count; ++i)
        {
            for (int j = 1; j <= 4; ++j)
            {
                sortedOrbList = upgrader.inventory.GetItemTypesTier(orbTypes[i], (ItemTier)j);
                if (sortedOrbList != null)
                {
                    foreach (var currOrb in sortedOrbList)
                    {
                        orbList[count].SetInfo(currOrb.ItemId);
                        orbList[count].gameObject.SetActive(true);

                        int index = count;
                        orbList[count].GetComponentInChildren<Button>().onClick.AddListener(() => upgrader.SelectOrbInPanel(index));
                        count++;
                    }
                }
            }
        }


        reset = false;
    }

    public void ResetOn()
    {
        reset = true;
    }
}
