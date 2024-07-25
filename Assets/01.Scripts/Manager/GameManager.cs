using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    private InputActionAsset joystickAction;

    public int CurrentStage { get; private set; } = 1;

    // 로딩 UI 
    private GameObject loadingUI;

    private void Awake()
    {
        Addressables.InstantiateAsync(Defines.loadingUI).Completed += (loadUIGo) =>
        {
            loadingUI = loadUIGo.Result;
            loadingUI.SetActive(false);
            DontDestroyOnLoad(loadingUI);
        };

        Addressables.LoadAssetAsync<InputActionAsset>(Defines.joystick).Completed += (joystick) =>
        {
            joystickAction = joystick.Result;
            joystickAction.Disable();
        };
    }

    // Defines 에서 호출 ex) Defines.main 
    public void LoadSceneAsync(string sceneName)
    {
        loadingUI.SetActive(true);

        Addressables.LoadSceneAsync(sceneName).Completed += (op) =>
        {
            loadingUI.SetActive(false);
        };
    }

    public void StartGame()
    {
        LoadSceneAsync(Defines.inGame);
    }

    public void ChangeStage(int stage)
    {
        CurrentStage = stage;
    }

}
