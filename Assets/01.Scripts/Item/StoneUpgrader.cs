using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoneUpgrader : MonoBehaviour
{
    public Button craftButton;
    public Draggable stonePrefab;
    public Draggable orbPiecePrefab;
    public Draggable orbPrefab;
    public ItemSlotUI slotPrefab;
    public ItemSlotUI upgradeSlot;

    public GameObject content;
    public List<ItemSlotUI> slots = new();

    private GameObject currentItem;
    private Draggable item;
    private List<Draggable> itemList;


    private void OnEnable()
    {
        //int stoneNum = GameManager.Instance.currSaveData.reinforce_Stone;
        //int orbPieceNum = GameManager.Instance.currSaveData.orb_Piece;
        //int orbNum = GameManager.Instance.currSaveData.orb_Normal;

        //int draggableNum = stoneNum + orbPieceNum + orbNum;

        //for (int i = 0; i < draggableNum; i++)
        //{
        //    slots.Add(Instantiate(slotPrefab));
        //    slots[i].transform.SetParent(content.transform);

        //    if(i < stoneNum)
        //    {
        //        item = Instantiate(stonePrefab);
        //    }
        //    else if(i >= stoneNum && i < stoneNum + orbNum)
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

    }






}
