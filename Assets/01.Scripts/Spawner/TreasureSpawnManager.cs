using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TreasureSpawnManager : MonoBehaviour
{
    public List<Treasure> treasures = new();
    public Well well = null;
    public readonly int treasuresCount = 3;

    private float minRadius = 100f;

    private float treasuresRadius = 50f;

    private TreasureData treasureData = null;
    public WellIndicator wellIndicator;

    private float wallSpace = 1f;
    private float currentSize = 0;

    private PlayerEquipLoader equipLoader;
    private InGameInventory inventory;

    private List<int> guardianId;
    private int currentGuardianIndex = 0;

    private PlayerSubject playerSubject;

    private float bonusProb = 60f;

    private void Start()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();

        treasureData = DataTableManager.Instance.Get<TreasureTable>(DataTableManager.treasure)
            .GetTreasure(GameManager.Instance.CurrentStage.ToString()); // stage name

        var stageData = DataTableManager.Instance.Get<BossStageTable>(DataTableManager.stageBoss)
            .GetBossData(GameManager.Instance.CurrentStage.ToString());

        guardianId = stageData.GetGuardianId();

        SpawnWall();

        // ���� ���� 
        for (int i = 0; i < treasuresCount; i++)
        {
            SetTreasure(treasureData);
        }

        // �칰 ����
        SpawnWell();
    }

    public Vector2 SetPosition(Treasure tr = null)
    {
        bool isSame = false;

        Vector2 randomPos = Vector2.zero;

        while (!isSame)
        {
            float randomDistance = Random.Range(minRadius, Defines.maxMapSize);

            randomPos = (Random.insideUnitCircle.normalized) * randomDistance;

            // �� ������ �������� üũ
            if (randomPos.x <= Defines.maxMapSize - 5 && randomPos.x >= -Defines.maxMapSize - 5 &&
                    randomPos.y <= Defines.maxMapSize - 5 && randomPos.y >= -Defines.maxMapSize - 5)
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
            for (int i = 1; i < stones.Count; i++)
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

                    // ȸ��

                    Addressables.InstantiateAsync(Defines.healItem).Completed += (x) =>
                    {
                        treasure.AddItem(x.Result);
                    };

                    // �ڼ�
                    Addressables.InstantiateAsync(Defines.magnet).Completed += (x) =>
                    {
                        treasure.AddItem(x.Result);
                    };

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

                    // ����� 
                    if (guardianId.Count > currentGuardianIndex)
                    {
                        var bossData = DataTableManager.Instance.Get<BossTable>
                        (DataTableManager.boss).GetBossData(guardianId[currentGuardianIndex].ToString());

                        Addressables.InstantiateAsync(bossData.Boss_Prefab).Completed
                            += (x) =>
                        {
                            var go = x.Result;
                            go.GetComponent<Boss>().Initialize(playerSubject, bossData);
                            go.SetActive(false);
                            treasure.guardian = go;
                        };

                        currentGuardianIndex++;
                    }

                    break;
                }
            }

        };

    }

    private void TreasureSpawnManager_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        throw new System.NotImplementedException();
    }

    // �� ��Ÿ�� ����
    public void SpawnWall()
    {
        Transform walls = new GameObject("walls").transform;

        // �� ���� ��
        currentSize = -Defines.maxMapSize;
        while (true)
        {
            Vector2 pos = new Vector2(currentSize, Defines.maxMapSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.identity, walls);

            if (currentSize >= Defines.maxMapSize)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // �� ���� �Ʒ�
        currentSize = -Defines.maxMapSize;
        while (true)
        {
            Vector2 pos = new Vector2(currentSize, -Defines.maxMapSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.identity, walls);

            if (currentSize >= Defines.maxMapSize)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // �� ���� ����
        currentSize = -Defines.maxMapSize;
        while (true)
        {
            Vector2 pos = new Vector2(-Defines.maxMapSize, currentSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.Euler(new Vector3(0f, 0f, 90f)), walls);

            if (currentSize >= Defines.maxMapSize)
            {
                break;
            }

            currentSize += wallSpace;
        }

        // �� ���� ������
        currentSize = -Defines.maxMapSize;
        while (true)
        {
            Vector2 pos = new Vector2(Defines.maxMapSize, currentSize);
            Addressables.InstantiateAsync(Defines.obstacles, pos, Quaternion.Euler(new Vector3(0f, 0f, 90f)), walls);

            if (currentSize >= Defines.maxMapSize)
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
