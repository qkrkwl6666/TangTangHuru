using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public int stage = 1;

    public void MoveToStage()
    {
        SceneManager.LoadScene(stage);
    }

    public void MoveToLobby()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
