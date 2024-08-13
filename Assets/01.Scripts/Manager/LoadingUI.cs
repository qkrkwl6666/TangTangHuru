using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance.loadingUI != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
