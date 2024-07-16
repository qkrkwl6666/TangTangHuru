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

    // 보물 상자
    private float treasureRadius = 1f; // 보물 아이템 거리
    private float treasureTime = 0f;
    private float treasureDuration = 3f;
    private bool opening = false;

    private List<Treasure> treasureList;
    private Treasure targetTreasure = null;

    public GameObject treasurePrefab;
    private Slider treasureBar;

    private void Awake()
    {
        hitCollider = new Collider[maxCollider];
        treasureList = GameObject.FindWithTag("TreasureSpawnManager")
            .GetComponent<TreasureSpawnManager>().treasures;

        treasureBar = Instantiate(treasurePrefab, transform).GetComponentInChildren<Slider>();
        treasureBar.gameObject.SetActive(false);
    }

    public void Update()
    {
        time += Time.deltaTime;

        if(time >= duration)
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
            if (Vector2.Distance(treasure.transform.position, transform.position) <= treasureRadius)
            {
                opening = true;
                treasureBar.gameObject.SetActive(true);
                targetTreasure = treasure;
                break;
            }
            else
            {
                opening = false;
            }
        }

        if (!opening) return;

        treasureTime += Time.deltaTime;

        float value = Mathf.InverseLerp(0f, treasureDuration, treasureTime);
        treasureBar.value = value;

        if(treasureTime >= treasureDuration)
        {
            treasureBar.gameObject.SetActive(false);
            treasureTime = 0f;
            opening = false;
            targetTreasure.UseItem();
            treasureList.Remove(targetTreasure);
            targetTreasure = null;
        }
    }
}
