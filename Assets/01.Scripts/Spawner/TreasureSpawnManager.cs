using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TreasureSpawnManager : MonoBehaviour
{
    public List<Treasure> treasures = new ();
    public readonly int treasuresCount = 3;

    private float minRadius = 150f;
    private float maxRadius = 350f;
    private float treasuresRadius = 30f;

    private TreasureData treasureData = null;

    private void Awake()
    {
        treasureData = DataTableManager.Instance.Get<TreasureTable>(DataTableManager.treasure)
            .GetTreasure(GameManager.Instance.CurrentStage.ToString());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SetTreasure(treasureData);
        }
    }

    public Vector2 SetPosition(Treasure tr)
    {
        bool isSame = false;

        Vector2 randomPos = Vector2.zero;

        while (!isSame) 
        {
            float randomDistance = Random.Range(minRadius, maxRadius);

            randomPos = (Random.insideUnitCircle.normalized) * randomDistance;

            // 맵 밖으로 나갔는지 체크
            if (randomPos.x <= 245f && randomPos.x >= -245f &&
                    randomPos.y <= 245f && randomPos.y >= -245f)
            {
                isSame = true;

                foreach (var treasure in treasures)
                {
                    if (treasure == tr) continue;

                    if (Vector2.Distance(treasure.transform.position,
                        tr.transform.position) <= treasuresRadius)
                    {
                        isSame = false; 
                        break;
                    }
                }
            }
        }

        return randomPos;
    }

    public void SetTreasure(TreasureData treasureData)
    {
        var stones = treasureData.GetEqupStones();

        float totalProbability = 0f;
        float currentProbability = 0f;

        foreach (var stone in stones) 
        {
            if (stone.id == -1) break;

            totalProbability += stone.prob;
        }

        Addressables.InstantiateAsync(Defines.treasure).Completed += (x) =>
        {
            var treasureGo = x.Result;

            float random = Random.Range(0, totalProbability);

            foreach (var stone in stones)
            {
                currentProbability += stone.prob;

                if (random <= currentProbability)
                {
                    var itemData = DataTableManager.Instance.Get<ItemTable>
                        (DataTableManager.item).GetItemData(stone.id.ToString());

                    var treasure = treasureGo.AddComponent<Treasure>();
                    // 장비 아이템 , 강화석 , 자석
                    var equipStone = new EquipmentGemstone(itemData);

                    int rand = Random.Range(treasureData.Min_Re_Stone, treasureData.Max_Re_Stone + 1);

                    for(int i = 0; i < rand; i++)
                    {
                        var re = new ReinforcedStone();
                        treasure.AddItem(re);
                    }

                    treasure.AddItem(equipStone);
                    treasures.Add(treasure);

                    treasureGo.transform.position = SetPosition(treasure);
                    break;
                }
            }

        };

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 150);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 350);
    }        
}
