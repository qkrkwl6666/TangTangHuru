using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public void OnStartButton()
    {
        GameManager.Instance.StartGame();
    }
}
