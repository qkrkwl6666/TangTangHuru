using UnityEngine;

public class SingletonAwake : MonoBehaviour
{
    private void Awake()
    {
        var saveMgr = SaveManager.Instance;

        var dtm = DataTableManager.Instance;

        var gm = GameManager.Instance;

        Application.targetFrameRate = 200;
    }


}
