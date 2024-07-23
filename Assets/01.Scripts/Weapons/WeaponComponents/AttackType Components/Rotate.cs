using UnityEngine;

public class Rotate : MonoBehaviour, IProjectile
{
    private float angle = 0f;

    float timer = 0f;

    IAimer currAimer;
    Transform parentTransform;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        parentTransform = currAimer.Player.transform;
    }

    private void OnEnable()
    {
        angle = (360f / currAimer.TotalCount) * currAimer.Index;
        transform.localScale = new Vector3(Size, Size);
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        } 
        else
        {
            float radians = angle * Mathf.Deg2Rad;
             
            float x = parentTransform.position.x + Mathf.Cos(radians) * Range;
            float y = parentTransform.position.y + Mathf.Sin(radians) * Range;

            Vector3 newPosition = new Vector3(x, y, 0);
            transform.position = newPosition;

            Vector3 direction = (transform.position - parentTransform.position).normalized;
            transform.up = direction;  // 오브젝트가 항상 바깥을 보도록

            angle += Speed * Time.deltaTime;
            timer += Time.deltaTime;
        }
    }



}
