using UnityEngine;

public class TilemapReposition : MonoBehaviour, IPlayerObserver
{
    public float TileSize = 30f;

    private PlayerSubject playerSubject;
    private Transform playerTransform;

    private void Start()
    {
        playerSubject = GameObject.FindWithTag("PlayerSubject").GetComponent<PlayerSubject>();
        playerSubject.AddObserver(this);
    }

    private void Update()
    {
        float diffX = playerTransform.position.x - transform.position.x;
        float diffY = playerTransform.position.y - transform.position.y;

        if (Mathf.Abs(diffX) > TileSize)
        {
            float dirX = diffX > 0 ? 1 : -1;
            transform.Translate(Vector3.right * dirX * TileSize * 2);
        }
        if (Mathf.Abs(diffY) > TileSize)
        {
            float dirY = diffY > 0 ? 1 : -1;
            transform.Translate(Vector3.up * dirY * TileSize * 2);
        }
    }

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }


    private void OnDestroy()
    {
        if (playerTransform == null) return;

        playerSubject.RemoveObserver(this);
    }
}
