using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class GameManager : Singleton<GameManager>
{
    public int CurrentStage { get; set; } = 1;

    public string currentWeapon = "OneSword";

    // 플레이어 장착
    public Dictionary<PlayerEquipment, (Item, GameObject ItemSlot)> playerEquipment = new();

    // 로딩 UI 
    public GameObject loadingUI;

    // 인게임 세이브 아이템
    
    // 임시 용도
    public string characterSkin = Defines.body033;
    public string weaponSkin = Defines.weapon005;

    // UI 
    public MainInventory mainInventory;

    // 인 게임 아이템 저장 컨테이너
    public List<IInGameItem> inGameItems = new ();

    // 인게임 에서 메인 씬 이동후 로딩 완료 시 호출
    public void InGameItemToMainItem()
    {
        foreach (var item in inGameItems) 
        {
            mainInventory.MainInventoryAddItem(item.ItemId.ToString());
        }

        inGameItems.Clear();
    }

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

        InGameItemToMainItem();
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
