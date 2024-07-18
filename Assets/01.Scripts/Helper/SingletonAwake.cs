using UnityEngine;

public class SingletonAwake : MonoBehaviour
{
    private void Awake()
    {
        var dtm = DataTableManager.Instance;

        var gm = GameManager.Instance;

        //Application.targetFrameRate = 60;

    }
}
