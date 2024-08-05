using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbCrafter : MonoBehaviour
{
    public TextMeshProUGUI stoneCountText;
    public Button craftButton;
    public List<Image> gaige;

    private int gaigeNum = 0; //저장요소
    private int stoneCount = 0;

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
        stoneCount = GameManager.Instance.currSaveData.reinforce_Stone;
        stoneCountText.text = stoneCount.ToString();
    }

    private void Craft()
    {
        if (!CheckCraftable())
            return;

        GameManager.Instance.currSaveData.reinforce_Stone -= 3;

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
        int stoneCount = GameManager.Instance.currSaveData.reinforce_Stone;
        if (stoneCount < 3)
        {
            Debug.Log("강화석 부족!");
            return false;
        }

        return true;
    }

    private void CraftSuccess()
    {
        if(gaigeNum < 5)
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
            GameManager.Instance.currSaveData.orb_Atk_Rare++;
        }
        Debug.Log("제작 성공!");
    }

    private void CraftFail()
    {
        Debug.Log("제작 실패..");
    }

}
