using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAwake : MonoBehaviour
{
    private void Awake()
    {
        var dtm = DataTableManager.Instance;
        Application.targetFrameRate = 60;
    }
}
