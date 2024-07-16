using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetection : MonoBehaviour
{
    private LinkedList<GameObject> followMeItems = new ();
    private List<GameObject> removeItems = new ();

    public LayerMask itemLayerMask;

    private float time = 0f;
    private float duration = 0.1f;

    private float radius = 3f; // 플레이어 아이템 거리

    private int maxCollider = 20;
    private Collider[] hitCollider;

    private float followSpeed = 5f;

    private InGameUI gameUI;

    // 보물 상자
    private float targetDistance = 1f; // 보물 아이템 거리
    private float treasureTime = 0f;
    private float treasureDuration = 3f;
    private bool opening = false;

    private List<Treasure> treasureList;
    private Treasure targetTreasure = null;

    // 보물 레이더
    private float treasureDistance = 100f;
    private float prevTreasureDistance = int.MaxValue;
    private Treasure radarTreasure = null;

    private void Awake()
    {
        hitCollider = new Collider[maxCollider];
        treasureList = GameObject.FindWithTag("TreasureSpawnManager")
            .GetComponent<TreasureSpawnManager>().treasures;

        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
    }

    public void Update()
    {
        time += Time.deltaTime;
        prevTreasureDistance = int.MaxValue;
        targetTreasure = null;

        if (time >= duration)
        {
            Physics.OverlapSphereNonAlloc(transform.position, radius, hitCollider, itemLayerMask);

            foreach(var item in hitCollider)
            {
                if (item == null) continue;
                Debug.Log(item.name);
                followMeItems.AddLast(item.gameObject);
            }
            time = 0f;
        }

        foreach(var item in followMeItems)
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

        for(int i = 0; i < hitCollider.Length; i ++)
        {
            hitCollider[i] = null;
        }

        // 보물 상자 체크
        if (treasureList == null) return;

        foreach(var treasure in treasureList)
        {
            var distacne = Vector2.Distance(treasure.transform.position, transform.position);

            if(distacne <= treasureDistance && distacne <= prevTreasureDistance)
            {
                prevTreasureDistance = distacne;
                radarTreasure = treasure;
            }

            if (distacne <= targetDistance)
            {
                opening = true;
                gameUI.SetActiveTreasureBar(true);
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
            gameUI.SetActiveTreasureBar(false);
            gameUI.UpdateTreasureBar(0f);
            treasureTime = 0f;
            return;
        }

        treasureTime += Time.deltaTime;

        float value = Mathf.InverseLerp(0f, treasureDuration, treasureTime);
        //treasureBar.value = value;
        gameUI.UpdateTreasureBar(value);

        if (treasureTime >= treasureDuration)
        {
            gameUI.SetActiveTreasureBar(false);
            gameUI.UpdateTreasureBar(0f);
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
}
