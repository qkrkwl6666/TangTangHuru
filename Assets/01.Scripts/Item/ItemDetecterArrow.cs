using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetecterArrow : MonoBehaviour
{
    private PlayerEquipLoader equipLoader;
    private ItemDetection detecter;

    private GameObject arrowPrefab;
    private List<GameObject> arrows = new();
    private List<Transform> targetTransform = new();

    private void Awake()
    {
        equipLoader = GetComponentInParent<PlayerEquipLoader>();
        detecter = GetComponentInParent<ItemDetection>();
    }
    void Start()
    {
        arrows.Add(Instantiate(arrowPrefab));
        arrows.Add(Instantiate(arrowPrefab));



    }

    void Update()
    {
        
    }

    private void SetRandomTreasureTransform()
    {
        var treasureList = detecter.GetTreasureList();
        targetTransform.Add(treasureList[Random.Range(0, 3)].transform);
    }

    private void PointTargetTreasure()
    {

        
    }
}
