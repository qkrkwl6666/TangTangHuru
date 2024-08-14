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

    private PlayerEquipLoader equipLoader;
    private InGameInventory inventory;

    private float bonusProb = 60f;

    private void Start()
    {
        treasureData = DataTableManager.Instance.Get<TreasureTable>(DataTableManager.treasure)
            .GetTreasure((GameManager.Instance.CurrentStage).ToString()); // stage name

        SpawnWall();

        // ���� ���� 
        for (int i = 0; i < treasuresCount; i++)
        {
            SetTreasure(treasureData);
        }

        // �칰 ����
        SpawnWell();
    }

    private void OnDestroy()
    {
        inventory.AllCoreCollected -= MoveWellPosition;
    }

    public Vector2 SetPosition(Treasure tr = null)
    {
        bool isSame = false;

        Vector2 randomPos = Vector2.zero;

        while (!isSame)
        {
            float randomDistance = Random.Range(minRadius, maxRadius);

            randomPos = (Random.insideUnitCircle.normalized) * randomDistance;

            // �� ������ �������� üũ
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
        //��Ʈȿ�� ����(�칰 ���� �̵�)
        equipLoader = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipLoader>();
        int armorSet = equipLoader.GetArmorSetType();
        if (armorSet == 3)
        {
            inventory = GameObject.FindGameObjectWithTag("InGameInventory").GetComponent<InGameInventory>();
            inventory.AllCoreCollected += MoveWellPosition;
        }

        var stones = treasureData.GetEqupStones();
        //��Ʈȿ�� ����(���� Ȯ�� ����)
        if (armorSet == 1)
        {
            for(int i = 1; i < stones.Count; i++)
            {
                if (stones[i].id == -1) break;
                var newProb = stones[i].prob + bonusProb;
                stones[i] = (stones[i].id, newProb);
            }
        }
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
                    // ��� ���� , ��ȭ�� , �ڼ�

                    // ��� ���� ����
                    Addressables.InstantiateAsync(itemData.Prefab_Id).Completed +=
                    (x) =>
                    {
                        var stone = x.Result;
                        var equip = stone.AddComponent<EquipmentGemstone>();
                        equip.Init(itemData);

                        treasure.AddItem(stone);
                    };

                    // ��ȭ�� ����
                    int rand = Random.Range(treasureData.Min_Re_Stone, treasureData.Max_Re_Stone + 1);

                    //��Ʈȿ�� ����
                    if (armorSet == 4)
                    {
                        rand += 2;
                    }

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

    // �� ��Ÿ�� ����
    public void SpawnWall()
    {
        Transform walls = new GameObject("walls").transform;

        // �� ���� ��
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

        // �� ���� �Ʒ�
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

        // �� ���� ����
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

        // �� ���� ������
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

    public void MoveWellPosition()
    {
        var dir = Random.insideUnitCircle;
        dir = dir.normalized * 3f;
        well.transform.position = (Vector2)equipLoader.transform.position + dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 150);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 250);
    }
}
