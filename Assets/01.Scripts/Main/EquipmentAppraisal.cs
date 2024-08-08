using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAppraisal : MonoBehaviour
{
    public MainInventory mainInventory;

    private Dictionary<ItemTier, GameObject> gemStoneSlotUI = new();

    public Transform content;

    private void Start()
    {
        
    }


    public void RefreshGemStoneSlotUI()
    {

    }
}
