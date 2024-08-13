using System.Collections.Generic;
using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    private LinkedList<GameObject> followMeItems = new();
    private List<GameObject> removeItems = new();

    public LayerMask itemLayerMask;

    private float time = 0f;
    private float duration = 0.1f;

    private float radius = 3f; // 플레이어 아이템 거리

    private int maxCollider = 20;
    private Collider[] hitCollider;

    private float followSpeed = 5f;

    private InGameUI gameUI;

    public TreasureBar treasureBarPrefab;
    private GameObject treasureBar;

    // 보물 상자
    private float targetDistance = 1f; // 보물 아이템 거리
    private float treasureTime = 0f;
    private float treasureDuration = 3f;
    private bool opening = false;

    private List<Treasure> treasureList;
    private Treasure targetTreasure = null;

    // 보물 레이더
    private float treasureDistance = 130f;
    private float prevTreasureDistance = int.MaxValue;
    private Treasure radarTreasure = null;

    private void Awake()
    {
        hitCollider = new Collider[maxCollider];
        treasureList = GameObject.FindWithTag("TreasureSpawnManager")
            .GetComponent<TreasureSpawnManager>().treasures;

        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
        var tBar = Instantiate(treasureBarPrefab);
        treasureBar = tBar.gameObject;
        treasureBar.transform.SetParent(gameObject.transform, false);
    }

    public void Update()
    {
        var bar = treasureBar.GetComponent<TreasureBar>();
        time += Time.deltaTime;
        prevTreasureDistance = int.MaxValue;
        targetTreasure = null;

        if (time >= duration)
        {
            Physics.OverlapSphereNonAlloc(transform.position, radius, hitCollider, itemLayerMask);

            foreach (var item in hitCollider)
            {
                if (item == null) break;

                if (!followMeItems.Contains(item.gameObject))
                {
                    followMeItems.AddLast(item.gameObject);
                }

            }
            time = 0f;
        }

        foreach (var item in followMeItems)
        {
            var dir = (transform.position - item.transform.position).normalized;
            item.transform.Translate(dir * Time.deltaTime * followSpeed);

            if (Vector2.Distance(item.transform.position, transform.position) < 1f)
            {
                removeItems.Add(item.gameObject);
            }
        }

        // 아이템 흭득 
        foreach (var item in removeItems)
        {
            item.GetComponent<IInGameItem>().UseItem();
            followMeItems.Remove(item);

            // Todo : 임시 아이템 비활성화
            item.SetActive(false);
        }

        removeItems.Clear();

        for (int i = 0; i < hitCollider.Length; i++)
        {
            hitCollider[i] = null;
        }

        // 보물 상자 체크
        if (treasureList == null) return;

        foreach (var treasure in treasureList)
        {
            var distance = Vector2.Distance(treasure.transform.position, transform.position);

            if (distance <= treasureDistance && distance <= prevTreasureDistance)
            {
                prevTreasureDistance = distance;
                radarTreasure = treasure;
            }

            if (distance <= targetDistance)
            {
                opening = true;
                bar.SetActiveTreasureBar(true);
                targetTreasure = treasure;
                break;
            }
            else
            {
                opening = false;
            }
        }

        Radar();

        if (!opening)
        {
            bar.SetActiveTreasureBar(false);
            bar.UpdateTreasureBar(0f);
            treasureTime = 0f;
            return;
        }

        treasureTime += Time.deltaTime;

        float value = Mathf.InverseLerp(0f, treasureDuration, treasureTime);
        //treasureBar.value = value;
        bar.UpdateTreasureBar(value);

        if (treasureTime >= treasureDuration)
        {
            bar.SetActiveTreasureBar(false);
            bar.UpdateTreasureBar(0f);
            treasureTime = 0f;
            opening = false;
            targetTreasure.UseItem();
            treasureList.Remove(targetTreasure);
            targetTreasure = null;
            radarTreasure = null;
        }

    }

    public void Radar()
    {
        // 레이더  
        if (radarTreasure == null)
        {
            gameUI.UpdateRadarBar(0f);
            return;
        }

        var distacne = Vector2.Distance(radarTreasure.transform.position, transform.position);
        float disValue = Mathf.InverseLerp(treasureDistance, 1f, distacne);

        gameUI.UpdateRadarBar(disValue);
    }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, treasureDistance);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, 20f);
    // }
}
