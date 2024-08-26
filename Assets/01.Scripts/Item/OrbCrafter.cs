using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbCrafter : MonoBehaviour
{
    public OrbUpgrader orbUpgrader;
    public MainInventory inventory;
    public TextMeshProUGUI stoneCountText;
    public TextMeshProUGUI stonePersent;
    public OrbNoticePanel popUp_Notice;
    public Button craftButton;
    public List<Image> gaige;

    private int gaigeNum = 0; //저장요소
    private int stoneCount = 0;
    private List<int> orbIdList = new List<int> { 610001, 610101, 610201, 610301 };

    private int createPersent = 56;

    void Start()
    {
        LoadGaige();
        foreach (var obj in gaige)
        {
            obj.color = Color.clear;
        }

        SetCount();
        craftButton.onClick.AddListener(Craft);

        for (int i = 0; i < gaigeNum; i++)
        {
            gaige[i].color = Color.yellow;
        }

        stonePersent.text = $"{createPersent}%"; ;
    }

    private void OnEnable()
    {
        SetCount();
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

        inventory.RemoveItem(ItemType.ReinforcedStone, 0, 3);
        if (Random.Range(0, 100) < createPersent)
        {
            CraftSuccess();
        }
        else
        {
            CraftFail();
        }
        inventory.RefreshItemSlotUI();
        orbUpgrader.popUp_OrbPanel.ResetOn();
        SetCount();
    }

    private bool CheckCraftable()
    {
        if (stoneCount < 3)
        {
            Debug.Log("강화석 부족!");
            return false;
        }

        return true;
    }

    private void CraftSuccess()
    {
        int rndIndex = Random.Range(0, orbIdList.Count);

        if (gaigeNum < 4)
        {
            gaige[gaigeNum].color = Color.yellow;
            gaigeNum++;
            SoundManager.Instance.PlaySound2D("success");
        }
        else
        {
            gaigeNum = 0;
            foreach (var obj in gaige)
            {
                obj.color = Color.clear;
            }
            inventory.MainInventoryAddItem(orbIdList[rndIndex].ToString());

            var orbData = DataTableManager.Instance.Get<ItemTable>
            (DataTableManager.item).GetItemData(orbIdList[rndIndex].ToString());

            popUp_Notice.SetInfo(orbData);
            popUp_Notice.gameObject.SetActive(true);

            SoundManager.Instance.PlaySound2D("orb");
        }
        SaveGaige();


    }

    private void CraftFail()
    {
        SoundManager.Instance.PlaySound2D("failed");
    }

    public void SaveGaige()
    {
        SaveManager.SaveDataV1.gaige = gaigeNum;
    }
    public void LoadGaige()
    {
        gaigeNum = SaveManager.SaveDataV1.gaige;
    }

    public void CreatePersentIncrease(int num)
    {
        createPersent += num;
    }
}
