using UnityEngine;

public class ReflectingShoot : MonoBehaviour, IProjectile
{
    public IAimer currAimer;
    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    private Vector2 dir;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector2 currPoint;
    private float timer = 0f;

    Vector3 currPosition = Vector3.zero;

    private void Awake()
    {
        currAimer = GetComponent<IAimer>();
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    private void OnEnable()
    {
        transform.position = currAimer.Player.transform.position;
        dir = currAimer.AimDirection();
    }

    void Update()
    {

        CheckBounds();


        if (timer > currAimer.LifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void CheckBounds()
    {
        currPoint = (Vector2)currAimer.Player.transform.position;
        Vector3 position = transform.position;

        if (position.x > currPoint.x + screenBounds.x || position.x < currPoint.x - screenBounds.x)
        {
            dir.x = -dir.x;
            position.x = Mathf.Clamp(position.x, currPoint.x - screenBounds.x, currPoint.x + screenBounds.x);
        }
        if (position.y > currPoint.y + screenBounds.y || position.y < currPoint.y - screenBounds.y)
        {
            dir.y = -dir.y;
            position.y = Mathf.Clamp(position.y, currPoint.y - screenBounds.y, currPoint.y + screenBounds.y);
        }
        transform.position = position;

        transform.Translate(dir * Speed * Time.deltaTime);
    }
}
