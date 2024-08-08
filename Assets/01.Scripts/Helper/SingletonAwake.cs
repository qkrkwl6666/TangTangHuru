using UnityEngine;

public class SingletonAwake : MonoBehaviour
{
    private void Awake()
    {
        var gm = GameManager.Instance;

        var saveMgr = SaveManager.Instance;

        var dtm = DataTableManager.Instance;

        Application.targetFrameRate = 200;
    }


}
