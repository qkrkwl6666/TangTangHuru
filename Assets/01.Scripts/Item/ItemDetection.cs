using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    public bool raderOwner = true;

    private LinkedList<GameObject> followMeItems = new();
    private List<GameObject> removeItems = new();

    public LayerMask itemLayerMask;

    private float time = 0f;
    private float duration = 0.1f;

    private float radius = 1.5f; // �÷��̾� ������ �Ÿ�
    private float magnetRadius = 10000f;

    private int maxCollider = 30;
    private Collider[] hitCollider;

    private float followSpeed = 5f;

    private InGameUI gameUI;

    public TreasureBar treasureBarPrefab;
    private GameObject treasureBar;

    // ���� ����
    private float targetDistance = 1f; // ���� ������ �Ÿ�
    private float treasureTime = 0f;
    private float treasureDuration = 3f;
    private bool opening = false;

    private List<Treasure> treasureList;
    private Treasure targetTreasure = null;

    // ���� ���̴�
    private float treasureDistance = 130f;
    private float prevTreasureDistance = int.MaxValue;
    private Treasure radarTreasure = null;

    private bool isMagnet = false;

    Vector3 endScreenPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

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

        ItemDotween();

        for (int i = 0; i < hitCollider.Length; i++)
        {
            hitCollider[i] = null;
        }

        // ���� ���� üũ
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
        if (!raderOwner)
            return;

        // ���̴�  
        if (radarTreasure == null)
        {
            gameUI.UpdateRadarBar(0f);
            return;
        }

        var distance = Vector2.Distance(radarTreasure.transform.position, transform.position);
        float disValue = Mathf.InverseLerp(treasureDistance, 1f, distance);

        gameUI.UpdateRadarBar(disValue);
    }

    public void ItemDotween()
    {
        if (time >= duration)
        {
            if(!isMagnet)
                Physics.OverlapSphereNonAlloc(transform.position, radius, hitCollider, itemLayerMask);
            else
            {
                //Physics.OverlapSphereNonAlloc(transform.position, magnetRadius, hitCollider, itemLayerMask);
                var colider = Physics.OverlapSphere(transform.position, magnetRadius, itemLayerMask);

                foreach (var item in colider)
                {
                    if (item == null) break;

                    if (!followMeItems.Contains(item.gameObject))
                    {
                        followMeItems.AddLast(item.gameObject);

                        Vector3 startScreenPos = Camera.main.WorldToScreenPoint(item.transform.position);

                        DOTween.To(() => startScreenPos, x =>
                        {
                            item.transform.position = Camera.main.ScreenToWorldPoint(x);
                        }, endScreenPos, 1f)
                            .SetEase(Ease.InBack)
                            .SetUpdate(UpdateType.Fixed)
                            .OnComplete(() =>
                            {
                                removeItems.Add(item.gameObject);
                                item.gameObject.SetActive(false);
                                item.GetComponent<IInGameItem>().UseItem();
                            });
                    }

                }

                isMagnet = false;
                return;
            }

            foreach (var item in hitCollider)
            {
                if (item == null) break;

                if (!followMeItems.Contains(item.gameObject))
                {
                    followMeItems.AddLast(item.gameObject);

                    Vector3 startScreenPos = Camera.main.WorldToScreenPoint(item.transform.position);

                    DOTween.To(() => startScreenPos, x =>
                    {
                        item.transform.position = Camera.main.ScreenToWorldPoint(x);
                    }, endScreenPos, 0.5f)
                        .SetEase(Ease.InBack)
                        .SetUpdate(UpdateType.Fixed)
                        .OnComplete(() =>
                    {
                        removeItems.Add(item.gameObject);
                        item.gameObject.SetActive(false);
                        item.GetComponent<IInGameItem>().UseItem();
                    });
                }

            }
            time = 0f;
        }

        // ������ ŉ�� 
        foreach (var item in removeItems)
        {
            followMeItems.Remove(item);
        }

        removeItems.Clear();
    }

    public void MagnetOn()
    {
        isMagnet = true;
    }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, treasureDistance);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, 20f);
    // }

}
