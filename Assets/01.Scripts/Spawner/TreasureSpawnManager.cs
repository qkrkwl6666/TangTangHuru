using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TreasureSpawnManager : MonoBehaviour
{
    public List<Treasure> treasures = new();

    public Well well = null;

    public readonly int treasuresCount = 3;

    private float minRadius = 150f;
    private float maxRadius = 250f;
    private float treasuresRadius = 50f;

    private TreasureData treasureData = null;
    public WellIndicator wellIndicator;

    private readonly int maxMapSizeHeight = 250;
    private readonly int maxMapSizeWidth = 250;
    private float wallSpace = 1f;
    private float currentSize = 0;

    private void Awake()
    {
        treasureData = DataTableManager.Instance.Get<TreasureTable>(DataTableManager.treasure)
            .GetTreasure(GameManager.Instance.CurrentStage.ToString());

        SpawnWall();

        // 보상 스폰 
        for (int i = 0; i < treasuresCount; i++)
        {
            SetTreasure(treasureData);
        }

        // 우물 스폰
        SpawnWell();
    }

    public void Update()
    {

    }

    public Vector2 SetPosition(Treasure tr = null)
    {
        bool isSame = false;

        Vector2 randomPos = Vector2.zero;

        while (!isSame)
        {
            float randomDistance = Random.Range(minRadius, maxRadius);

            randomPos = (Random.insideUnitCircle.normalized) * randomDistance;

            // 맵 밖으로 나갔는지 체크
            if (randomPos.x <= maxMapSizeWidth - 5 && randomPos.x >= -maxMapSizeWidth - 5 &&
                    randomPos.y <= maxMapSizeHeight - 5 && randomPos.y >= -maxMapSizeHeight - 5)
            {
                isSame = true;

                foreach (var treasure in treasures)
                {
                    if (tr != null)
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

                    for (int i = 0; i < rand; i++)
                    {
                        Addressables.InstantiateAsync("Re_Stone").Completed +=
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

    // 맵 울타리 스폰
    public void SpawnWall()
    {
        Transform walls = new GameObject("walls").transform;

        // 벽 생성 위
        currentSize = -maxMapSizeWidth;
        while (true)
        {
            Vector2 pos = new Vector2(currentSize, maxMapSizeWidth);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.identity, walls);

            if (currentSize >= maxMapSizeWidth)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // 벽 생성 아래
        currentSize = -maxMapSizeWidth;
        while (true)
        {
            Vector2 pos = new Vector2(currentSize, -maxMapSizeWidth);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.identity, walls);

            if (currentSize >= maxMapSizeWidth)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // 벽 생성 왼쪽
        currentSize = -maxMapSizeHeight;
        while (true)
        {
            Vector2 pos = new Vector2(-maxMapSizeWidth, currentSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.Euler(new Vector3(0f, 0f, 90f)), walls);

            if (currentSize >= maxMapSizeHeight)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // 벽 생성 오른쪽
        currentSize = -maxMapSizeHeight;
        while (true)
        {
            Vector2 pos = new Vector2(maxMapSizeWidth, currentSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.Euler(new Vector3(0f, 0f, 90f)), walls);

            if (currentSize >= maxMapSizeHeight)
            {
                break;
            }

            currentSize += wallSpace;
        }
    }

    public void SpawnWell()
    {

        Addressables.InstantiateAsync(Defines.well).Completed += (x) =>
        {
            well = x.Result.GetComponent<Well>();
            well.transform.position = SetPosition();
            wellIndicator.target = well.transform;
        };

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 150);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 250);
    }
}
