using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public int CurrentStage { get; private set; } = 1;

    public string currentWeapon = "OneSword";

    // �ε� UI 
    private GameObject loadingUI;

    // �ӽ� �뵵
    public string characterSkin = Defines.body033;
    public string weaponSkin = Defines.weapon005;

    // �� ���� ������ ���� �����̳�
    private List<IInGameItem> inGameItems = new ();

    public void InGameItemClear()
    {
        inGameItems.Clear();
    }

    public void AddinGameItem(IInGameItem item)
    {
        inGameItems.Add(item);
    }


    private void Awake()
    {
        Addressables.InstantiateAsync(Defines.loadingUI).Completed += (loadUIGo) =>
        {
            loadingUI = loadUIGo.Result;
            loadingUI.SetActive(false);
            DontDestroyOnLoad(loadingUI);
        };
    }

    // Defines ���� ȣ�� ex) Defines.main 
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
