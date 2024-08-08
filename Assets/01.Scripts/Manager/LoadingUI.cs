using System.Collections;
using System.Collections.Generic;
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
