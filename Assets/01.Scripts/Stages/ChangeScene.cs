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
        //�׽�Ʈ��
        StopAllCoroutines();
        GameManager.Instance.LoadSceneAsync(Defines.mainScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
