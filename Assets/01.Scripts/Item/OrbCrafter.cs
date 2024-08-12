using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbCrafter : MonoBehaviour
{
    public MainInventory inventory;
    public TextMeshProUGUI stoneCountText;
    public Button craftButton;
    public List<Image> gaige;

    private int gaigeNum = 0; //������
    private int stoneCount = 0;
    private List<int> orbIdList = new List<int> { 610001, 610101, 610201, 610301 };

    void Start()
    {
        foreach (var obj in gaige)
        {
            obj.color = Color.clear;
        }

        SetCount();
        craftButton.onClick.AddListener(Craft);

        for(int i = 0; i < gaigeNum; i++)
        {
            gaige[i].color = Color.yellow;
        }
    }

    private void SetCount()
    {
        stoneCount = inventory.GetItemCount(ItemType.ReinforcedStone, 0);
        stoneCountText.text = stoneCount.ToString();
    }

    private void Craft()
    {
        if (!CheckCraftable())
            return;

        //Todo. �κ��丮 RemoveItem �Ϸ�� �߰�����.

        if (Random.Range(0, 100) < 56)
        {
            CraftSuccess();
        }
        else
        {
            CraftFail();
        }
        SetCount();
    }

    private bool CheckCraftable()
    {
        if (stoneCount < 3)
        {
            Debug.Log("��ȭ�� ����!");
            return false;
        }

        return true;
    }

    private void CraftSuccess()
    {
        int rndIndex = Random.Range(0, orbIdList.Count);

        if (gaigeNum < 5)
        {
            gaige[gaigeNum].color = Color.yellow;
            gaigeNum++;
        }
        else
        {
            gaigeNum = 0;
            foreach(var obj in gaige)
            {
                obj.color = Color.clear;
            }
            inventory.MainInventoryAddItem(orbIdList[rndIndex].ToString());
        }
        Debug.Log("���� ����!");
    }

    private void CraftFail()
    {
        Debug.Log("���� ����..");
    }

}
