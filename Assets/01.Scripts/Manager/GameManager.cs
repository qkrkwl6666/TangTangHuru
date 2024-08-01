using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SaveData
{
    public int player_Gold;
    public float player_Exp;
    public int stage_Record;

    public int reinforce_Stone;
    public int orb_Piece;
    public int orb_Normal;
    public int Orb_Rare;
}


public class GameManager : Singleton<GameManager>
{
    public SaveData currSaveData = new();
    public int CurrentStage { get; private set; } = 1;

    public string currentWeapon = "OneSword";

    // �ε� UI 
    private GameObject loadingUI;

    // �ӽ� �뵵
    public string characterSkin = Defines.body033;
    public string weaponSkin = Defines.weapon005;

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
