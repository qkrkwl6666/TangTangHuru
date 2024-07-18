using UnityEngine;

public class RandomTarget : MonoBehaviour, IAimer
{
    public GameObject player;
    public GameObject Player { get => player; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public int TotalCount { get; set; }
    public int Index { get; set; }

    private float range = 30f;

    public LayerMask targetLayer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetLayer = LayerMask.GetMask("Enemy");
    }

    public Vector3 AimDirection()
    {
        var targets = Physics2D.CircleCastAll(Player.transform.position, range, Vector2.zero, 0, targetLayer);

        Vector3 result = Vector3.zero;

        if (targets.Length != 0)
        {
            var index = Random.Range(0, targets.Length);
            result = targets[index].transform.position;
        }
        else
        {
            result = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }

        return result;
    }
}
