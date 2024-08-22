using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    public bool isTutorial = false;

    public bool raderOwner = true;

    private LinkedList<GameObject> followMeItems = new();
    private List<GameObject> removeItems = new();

    public LayerMask itemLayerMask;

    private float time = 0f;
    private float duration = 0.1f;

    private float radius = 1.5f; // 플레이어 아이템 거리
    private float magnetRadius = 10000f;

    private int maxCollider = 30;
    private Collider[] hitCollider;

    private float followSpeed = 5f;

    private InGameUI gameUI;

    public TreasureBar treasureBarPrefab;
    private TreasureBar treasureBar;

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

    // 세트효과 관련
    private PlayerEquipLoader equipLoader;
    private TreasureSpawnManager treasureManager;

    private bool isMagnet = false;
    Vector3 endScreenPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

    public TutorialGameInit tutorialGameInit;

    private void Awake()
    {
        hitCollider = new Collider[maxCollider];

        if (!isTutorial)
        {
            treasureManager = GameObject.FindWithTag("TreasureSpawnManager").GetComponent<TreasureSpawnManager>();
            treasureList = treasureManager.treasures;
        }

        gameUI = GameObject.FindWithTag("InGameUI").GetComponent<InGameUI>();
        treasureBar = Instantiate(treasureBarPrefab);
        treasureBar.gameObject.transform.SetParent(gameObject.transform, false);
    }

    private void Start()
    {
        equipLoader = GetComponentInParent<PlayerEquipLoader>();
        if (equipLoader == null)
            return;
        if (equipLoader.GetArmorSetType() == 7)
        {
            treasureDistance *= 1.3f;
        }
    }

    public void Update()
    {
        time += Time.deltaTime;
        prevTreasureDistance = int.MaxValue;
        targetTreasure = null;

        ItemDotween();

        for (int i = 0; i < hitCollider.Length; i++)
        {
            hitCollider[i] = null;
        }

        // 보물 상자 체크
        if (treasureList == null) return;

        foreach (var treasure in treasureList)
        {
            if (treasure == null) 
                continue;

            var distance = Vector2.Distance(treasure.transform.position, transform.position);

            if (distance <= treasureDistance && distance <= prevTreasureDistance)
            {
                prevTreasureDistance = distance;
                radarTreasure = treasure;
            }

            if (distance <= targetDistance)
            {
                opening = true;
                treasureBar.SetActiveTreasureBar(true);
                targetTreasure = treasure;
                break;
            }
            else
            {
                opening = false;
            }
        }

        if (raderOwner)
        {
            Radar();
        }

        if (!opening)
        {
            treasureBar.SetActiveTreasureBar(false);
            treasureBar.UpdateTreasureBar(0f);
            treasureTime = 0f;
            return;
        }

        treasureTime += Time.deltaTime;

        float value = Mathf.InverseLerp(0f, treasureDuration, treasureTime);
        //treasureBar.value = value;
        treasureBar.UpdateTreasureBar(value);

        //보물상자 열림
        if (treasureTime >= treasureDuration) 
        {
            OpenTreasure();
            Radar();
        }
    }

    public void Radar()
    {
        // 레이더  
        if (radarTreasure == null || !radarTreasure.gameObject.activeSelf)
        {
            gameUI.UpdateRadarBar(0f);
            return;
        }

        var distance = Vector2.Distance(radarTreasure.transform.position, transform.position);
        float disValue = Mathf.InverseLerp(treasureDistance, 1f, distance);

        gameUI.UpdateRadarBar(disValue);
    }

    public void OpenTreasure()
    {
        treasureBar.SetActiveTreasureBar(false);
        treasureBar.UpdateTreasureBar(0f);
        treasureTime = 0f;
        opening = false;
        targetTreasure.UseItem();
        treasureList.Remove(targetTreasure);
        targetTreasure = null;
        radarTreasure = null;


        //장비 세트 효과
        if (equipLoader.GetArmorSetType() == 6)
        {
            var playerExp = GetComponentInParent<PlayerExp>();
            var earnedExp = playerExp.GetRequiredExp();
            playerExp.EarnExp(earnedExp);
        }
    }

    public List<Treasure> GetTreasureList()
    {
        return treasureList;
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
                        endScreenPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
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
                    endScreenPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
                    //Debug.Log(endScreenPos);
                    DOTween.To(() => startScreenPos, x =>
                    {
                        item.transform.position = Camera.main.ScreenToWorldPoint(x);
                    }, endScreenPos, 0.5f)
                        .SetEase(Ease.InBack)
                        .SetUpdate(UpdateType.Fixed)
                        .OnComplete(() =>
                    {
                        if(!removeItems.Contains(item.gameObject))
                        {
                            removeItems.Add(item.gameObject);
                            item.GetComponent<IInGameItem>().UseItem();
                        }
                    });
                }

            }
            time = 0f;
        }

        // 아이템 흭득 
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

    public void SetTutorialTreasure(Treasure treasure)
    {
        treasureList = new()
        {
            treasure
        };
    }

}
