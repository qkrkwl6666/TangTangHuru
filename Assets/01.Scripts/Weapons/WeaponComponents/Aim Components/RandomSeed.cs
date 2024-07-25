using UnityEngine;

public class RandomSeed : MonoBehaviour, IAimer
{
    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private System.Random random;
    private int seed = 12345; // 고정된 시드 값
    private int currentIndex = 0;
    private Vector2[] precomputedVectors;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InitializeRandom(seed);
        PrecomputeVectors(100); // 100개의 벡터를 미리 계산
    }

    void InitializeRandom(int seed)
    {
        random = new System.Random(seed);
        currentIndex = 0;
    }

    void PrecomputeVectors(int count)
    {
        precomputedVectors = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            precomputedVectors[i] = SetRandomVector2D();
        }
    }
    Vector2 SetRandomVector2D()
    {
        float angle = (float)(random.NextDouble() * Mathf.PI * 2);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public Vector3 AimDirection()
    {
        Vector2 nextVector = precomputedVectors[currentIndex];
        currentIndex = (currentIndex + 1) % precomputedVectors.Length; // 인덱스 순환

        if (Index == TotalCount)
        {
            currentIndex++;
        }

        return nextVector;
    }

    public void Reset()
    {
        InitializeRandom(seed);
        PrecomputeVectors(precomputedVectors.Length);
    }
}
