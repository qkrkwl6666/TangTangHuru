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

    public void IObserverUpdate()
    {
        playerTransform = playerSubject.GetPlayerTransform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))
        {
            float diffX = playerTransform.position.x - transform.position.x;
            float diffY = playerTransform.position.y - transform.position.y;

            float dirX = diffX > 0 ? 1 : -1;
            float dirY = diffY > 0 ? 1 : -1;

            if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
            {
                transform.Translate(Vector3.right * dirX * TileSize * 2);
            }
            else
            {
                transform.Translate(Vector3.up * dirY * TileSize * 2);
            }
        }
    }


    private void OnDestroy()
    {
        if (playerTransform == null) return;

        playerSubject.RemoveObserver(this);
    }
}
