using UnityEngine;

public class LaserShoot : MonoBehaviour, IProjectile
{
    private LayerMask attackableMask;
    private LineRenderer laser;
    private RaycastHit2D laserHit;
    private RaycastHit2D laserHitGuardian;

    private IAimer currAimer;
    private Collider2D attackCollider;

    private float timer = 0f;
    private float redirectionTimer = 0f;

    private Vector2 endPoint;

    public float Range { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        attackableMask = LayerMask.GetMask("Enemy");
        currAimer = GetComponent<IAimer>();
        laser = GetComponent<LineRenderer>();
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        timer = 0f;
        redirectionTimer = 0f;
        SetDestination();
    }

    void FixedUpdate()
    {
        SetDestination();
        laser.SetPosition(0, currAimer.Player.transform.position);
        laser.SetPosition(1, endPoint);
        attackCollider.gameObject.transform.position = endPoint;

        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
            redirectionTimer += Time.deltaTime;
        }
    }

    void SetDestination()
    {
        laserHit = Physics2D.Raycast(currAimer.Player.transform.position, currAimer.AimDirection(), Range, attackableMask);
        //laserHitGuardian = Physics2D.Raycast(currAimer.Player.transform.position, currAimer.AimDirection(), Range, LayerMask.NameToLayer("Guardian"));
        
        if (laserHit.collider != null)
        {
            endPoint = laserHit.point;
        }
        else
        {
            endPoint = currAimer.Player.transform.position + currAimer.AimDirection() * Range;
        }
    }

}
