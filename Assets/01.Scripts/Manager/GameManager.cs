using System;
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

    // 플레이어 장착
    public Dictionary<PlayerEquipment, (Item, GameObject ItemSlot)> playerEquipment = new();

    // 로딩 UI 
    public GameObject loadingUI;
    
    // 임시 용도
    public string characterSkin = Defines.body033;
    public string weaponSkin = Defines.weapon005;

    // UI 
    public MainInventory mainInventory;

    // 인 게임 아이템 저장 컨테이너
    private List<IInGameItem> inGameItems = new ();

    public void InGameItemClear()
    {
        inGameItems.Clear();
    }

    private void Update()
    {
        
    }

    public void AddinGameItem(IInGameItem item)
    {
        inGameItems.Add(item);
    }

    private void Awake()
    {
        loadingUI = GameObject.FindWithTag("Loading");
        DontDestroyOnLoad(loadingUI);
    }

    private void Start()
    {
        Debug.Log("GameManagerStart");

        mainInventory = GameObject.FindWithTag("MainInventory")
            .GetComponent<MainInventory>();

        mainInventory.OnMainInventorySaveLoaded += InitSaveLoaded;
    }

    public void InitSaveLoaded()
    {
        // Todo : 코드 교체 필요 
        mainInventory = GameObject.FindWithTag("MainInventory")
            .GetComponent<MainInventory>();

        mainInventory.gameObject.SetActive(false);
        loadingUI.SetActive(false);
    }

    // Defines 에서 호출 ex) Defines.main 
    public void LoadSceneAsync(string sceneName)
    {
        loadingUI.SetActive(true);

        // 저장
        mainInventory.SaveMainInventory();

        Addressables.LoadSceneAsync(sceneName).Completed += (op) =>
        {
            //Todo : 메인 씬 이름 변경시 변경 필요
            if(sceneName != "InventoryScene")
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

    public void SceneSaveInventory()
    {


        //this.allItem = allItem;
        //this.playerEquipment = playerEquipment;
    }

}
