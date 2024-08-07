using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SaveData
{
    public int player_Gold;
    public float player_Exp;
    public int stage_Record;

    public int equip_GemStone;
    public int reinforce_Stone = 40;
    public int orb_Atk_Rare = 1;
    public int orb_Atk_Epic = 1;
    public int orb_Atk_Unique = 2;
    public int orb_Atk_Legend = 1;

}


public class GameManager : Singleton<GameManager>
{
    public SaveData currSaveData = new();
    public int CurrentStage { get; set; } = 1;

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
