using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LaserSkill : MonoBehaviour, IBossSkill
{
    public class LaserSetting
    {
        public float rotationTime = 0f; // 회전 시간
        public float rotationSpeed = 0f; // 레이더 회전 스피드
        public int laserCount = 0; // 레이저 개수
        public float yScale = 0f; // 레이저 y축 스케일
        public float laserRate = 0f; // 레이저 나오는 준비 시간
        public float laserDistance = 5f;

        public List<RotationType> rotationTypes = new List<RotationType>();

        public void SetData(int laserCount, float rotationTime,
            float yScale, float rotationSpeed, float lasetRate, float laserDistance)
        {
            this.laserCount = laserCount;
            this.rotationTime = rotationTime;
            this.yScale = yScale;
            this.rotationSpeed = rotationSpeed;
            this.laserRate = lasetRate;
            this.laserDistance = laserDistance;
        }

        public void SetData(LaserSetting laserSetting)
        {
            this.laserCount = laserSetting.laserCount;
            this.rotationTime = laserSetting.rotationTime;
            this.yScale = laserSetting.yScale;
            this.rotationSpeed = laserSetting.rotationSpeed;
            this.laserRate = laserSetting.laserRate;
            this.laserDistance = laserSetting.laserDistance;
            this.rotationTypes = laserSetting.rotationTypes;
        }
    }

    public enum RotationType
    {
        Right,
        Left,
    }

    private IObjectPool<GameObject> pool;

    public float Damage { get; private set; }
    public int SkillCount { get; set; }
    public bool IsChange { get; set; }
    public float SkillRate { get; set; }
    public float DamageFactor { get; set; }

    public LaserSetting laserSetting = new LaserSetting();

    private float time = 0f;

    public void Activate()
    {
        enabled = true;

        //레이저 생성
        for (int i = 0; i < laserSetting.laserCount; i++)
        {
            float angle = ((360 / laserSetting.laserCount) * i) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            var laserGo = pool.Get();
            laserGo.transform.up = dir;
            laserGo.transform.position = (Vector2)transform.position + dir * laserSetting.laserDistance;

            var laser = laserGo.GetComponent<Laser>();
            laser.laserSetting.SetData(laserSetting);
            laser.SetInitialAngle(angle * Mathf.Rad2Deg);
            laser.Init();
        }
    }

    public void DeActivate()
    {
        enabled = false;
        time = 0f;
        IsChange = false;
    }

    // 레이저 회전 방향 설정 해줘야함
    public void SetLaser(int laserCount,float rotationTime ,float yScale,
        float rotationSpeed, float laserDistance)
    {
        laserSetting.SetData(laserCount, rotationTime, yScale, rotationSpeed,
            SkillRate, laserDistance);
    }

    public void Initialize(BossSkillData bossSkillData, float damage)
    {
        Addressables.LoadAssetAsync<GameObject>(bossSkillData.Preafab_Id)
            .Completed += InstantiateLaser;

        SkillCount = bossSkillData.Skill_Count;
        DamageFactor = bossSkillData.Damage_Factor;
        SkillRate = bossSkillData.Skill_Rate;
        Damage = damage * DamageFactor;
        enabled = false;
    }

    public void SkillUpdate(float deltaTime)
    {
        time += deltaTime;

        if (time >= (SkillCount + laserSetting.laserRate)
            * laserSetting.rotationTypes.Count && !IsChange)
        {
            Debug.Log("IsChange");
            IsChange = true;
        }
    }

    public void InstantiateLaser(AsyncOperationHandle<GameObject> op)
    {
        var prefab = op.Result;

        pool = new ObjectPool<GameObject>
            (() =>
            {
                var go = Instantiate(prefab, transform);
                var laser = go.GetComponent<Laser>();
                laser.SetObjectPool(pool);
                laser.SetDamageTransform(Damage, transform);
                return go;
            },
            (x) =>
            {
                x.SetActive(true);
            },
            (x) =>
            {
                x.SetActive(false);
            },
            (x) => Destroy(x.gameObject),
            true, 10, 100);
    }
}
