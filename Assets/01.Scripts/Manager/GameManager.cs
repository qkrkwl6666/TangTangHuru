using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class GameManager : Singleton<GameManager>
{
    // tutorial
    public bool isTutorialSceneEnd = false;

    public int CurrentStage { get; set; } = 1;

    public string currentWeapon = "OneSword";

    // �뵆�젅�씠�뼱 �옣李�
    public Dictionary<PlayerEquipment, (Item, GameObject ItemSlot)> playerEquipment = new();

    // 濡쒕뵫 UI 
    public GameObject loadingUI;

    // �씤寃뚯엫 �꽭�씠釉� �븘�씠�뀥

    // �엫�떆 �슜�룄
    public string characterSkin = Defines.body033;
    public string weaponSkin = Defines.weapon005;

    // UI 
    public MainInventory mainInventory;

    // �씤 寃뚯엫 �븘�씠�뀥 ����옣 而⑦뀒�씠�꼫
    public List<IInGameItem> inGameItems = new();

    private int BGM_Index = 0;

    // �씤寃뚯엫 �뿉�꽌 硫붿씤 �뵮 �씠�룞�썑 濡쒕뵫 �셿猷� �떆 �샇異�
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
        // Todo : 肄붾뱶 援먯껜 �븘�슂 
        mainInventory = GameObject.FindWithTag("MainInventory")
            .GetComponent<MainInventory>();

        mainInventory.gameObject.SetActive(false);
        loadingUI.SetActive(false);

        InGameItemToMainItem();

        mainInventory.SaveInventory();
    }

    // Defines �뿉�꽌 �샇異� ex) Defines.main 
    public void LoadSceneAsync(string sceneName)
    {
        SoundManager.Instance.ClearSoundPlayerPool();

        loadingUI.SetActive(true);

        // Todo : 硫붿씤 �뵮 �씠由� 蹂�寃쎌떆 蹂�寃� �븘�슂

        if (sceneName != Defines.mainScene)
            mainInventory.SaveMainInventory();



        Addressables.LoadSceneAsync(sceneName).Completed += (op) =>
        {
            //Todo : 硫붿씤 �뵮 �씠由� 蹂�寃쎌떆 蹂�寃� �븘�슂

            if (sceneName != Defines.mainScene)
                loadingUI.SetActive(false);
            SoundManager.Instance.CreateTemporalObjects();
        };

        if (sceneName == Defines.mainScene)
        {
            BGM_Index = 0;
        }
        else
        {
            BGM_Index = 1;
            SoundManager.Instance.EnterStage();
        }
        Invoke("ChangeBGM", 2);
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

    public void ChangeBGM()
    {
        SoundManager.Instance.PlayerBGM(BGM_Index);
    }

    public void SaveGame()
    {
        mainInventory.SaveInventory();
    }

}
