using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Well : MonoBehaviour
{
    private InGameInventory inventory;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("InGameInventory").GetComponent<InGameInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Defines.player)
        {
            inventory.SaveItem(true);
        }
    }

}
