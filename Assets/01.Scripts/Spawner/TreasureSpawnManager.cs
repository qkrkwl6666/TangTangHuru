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
    private float treasuresRadius = 50f;

    private TreasureData treasureData = null;

    private void Awake()
    {
        treasureData = DataTableManager.Instance.Get<TreasureTable>(DataTableManager.treasure)
            .GetTreasure(GameManager.Instance.CurrentStage.ToString());

        for (int i = 0; i < treasuresCount; i++)
        {
            SetTreasure(treasureData);
        }
    }

    public void Update()
    {

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
                        randomPos) <= treasuresRadius)
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
                    // 장비 원석 , 강화석 , 자석

                    // 장비 원석 생성
                    Addressables.InstantiateAsync(itemData.Prefab_Id).Completed += 
                    (x) => 
                    {
                        var stone = x.Result;
                        var equip = stone.AddComponent<EquipmentGemstone>();
                        equip.Init(itemData);

                        treasure.AddItem(stone);
                    };

                    // 강화석 생성
                    int rand = Random.Range(treasureData.Min_Re_Stone, treasureData.Max_Re_Stone + 1);

                    for(int i = 0; i < rand; i++)
                    {
                        Addressables.InstantiateAsync("Normal_Re_Stone").Completed +=
                        (x) =>
                        {
                            var restoneGo = x.Result;
                            var reStone = restoneGo.AddComponent<ReinforcedStone>();
                            treasure.AddItem(restoneGo);
                        };
                    }

                    treasureGo.transform.position = SetPosition(treasure);
                    treasures.Add(treasure);
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
