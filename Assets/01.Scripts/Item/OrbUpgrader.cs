using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbUpgrader : MonoBehaviour
{
    public Button craftButton;

    public Image prefab_Stone;
    public Image prefab_Orb;
    public Image prefab_RareOrb;
    public Image prefab_EpicOrb;

    public ItemSlotUI slotPrefab;
    public ItemSlotUI upgradeSlot;
    public List<ItemSlotUI> slots = new();


    private void OnEnable()
    {
        int stoneNum = GameManager.Instance.currSaveData.reinforce_Stone;
        int orbPieceNum = GameManager.Instance.currSaveData.orb_Piece;
        int orbNum = GameManager.Instance.currSaveData.orb_Normal;

        int draggableNum = stoneNum + orbPieceNum + orbNum;

        //for (int i = 0; i < draggableNum; i++)
        //{
        //    slots.Add(Instantiate(slotPrefab));
        //    slots[i].transform.SetParent(content.transform);

        //    if (i < stoneNum)
        //    {
        //        item = Instantiate(stonePrefab);
        //    }
        //    else if (i >= stoneNum && i < stoneNum + orbNum)
        //    {
        //        item = Instantiate(orbPiecePrefab);
        //    }
        //    else
        //    {
        //        item = Instantiate(orbPrefab);
        //    }
        //    item.transform.SetParent(slots[i].transform);
        //    item.transform.position = slots[i].transform.position;
        //    itemList.Add(item);
        //}

        craftButton.onClick.AddListener(Craft);
    }

    private void OnDisable()
    {
        craftButton.onClick.RemoveAllListeners();
    }

    private void OnTransformChildrenChanged()
    {

    }

    private void Craft()
    {
    }
}
