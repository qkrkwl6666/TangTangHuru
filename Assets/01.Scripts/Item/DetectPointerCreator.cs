using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DetectPointerCreator : MonoBehaviour
{
    private PlayerEquipLoader equipLoader;
    private ItemDetection detecter;

    private GameObject pointerPrefab;
    private List<GameObject> pointers = new();

    private List<Treasure> treasures = new();

    private float stageTimer = 0f;
    private int armorSet = 0;

    private void Awake()
    {
        equipLoader = GetComponentInParent<PlayerEquipLoader>();
        detecter = GetComponent<ItemDetection>();
    }
    void Start()
    {
        armorSet = equipLoader.GetArmorSetType();

        if (armorSet != 2 && armorSet != 5)
            return;

        Addressables.LoadAssetAsync<GameObject>("TreasurePointer").Completed +=
            (obj) =>
            {
                pointerPrefab = obj.Result;
                pointers.Add(Instantiate(pointerPrefab, transform));
                pointers.Add(Instantiate(pointerPrefab, transform));

                foreach (var pointer in pointers)
                {
                    pointer.gameObject.SetActive(false);
                }

                if (armorSet == 2)
                {
                    Invoke("SetPointer", 0.5f);
                }
            };
    }

    void Update()
    {
        if (armorSet != 5)
            return;

        if(pointers.Count == 0) 
            return;

        if (Time.timeScale > 0f)
        {
            stageTimer += Time.deltaTime;
        }

        if(stageTimer > 3)
        {
            var pointerComponent = pointers[0].GetComponent<TreasurePointer>();
            pointerComponent.SetTarget(SetRandomTreasureTransform());
            pointerComponent.SetPivot(equipLoader.gameObject);
            pointerComponent.gameObject.SetActive(true);
            pointers.RemoveAt(0);
            stageTimer = 0f;
        }
    }

    private void SetPointer()
    {
        var pointerComponent = pointers[0].GetComponent<TreasurePointer>();
        pointerComponent.SetTarget(SetRandomTreasureTransform());
        pointerComponent.SetPivot(equipLoader.gameObject);
        pointerComponent.gameObject.SetActive(true);
    }

    private Treasure SetRandomTreasureTransform()
    {
        var treasureList = detecter.GetTreasureList();

        foreach (var treasure in treasureList)
        {
            if (treasure.gameObject.activeSelf)
            {
                treasures.Add(treasure);
            }
        }

        var picked = Random.Range(0, treasures.Count);
        var t = treasures[picked];
        treasures.Remove(treasures[picked]);

        return t;
    }
}
