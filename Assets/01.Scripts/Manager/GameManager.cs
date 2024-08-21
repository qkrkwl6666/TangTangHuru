using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class GameManager : Singleton<GameManager>
{
    public int CurrentStage { get; set; } = 1;

    public string currentWeapon = "OneSword";

    // �÷��̾� ����
    public Dictionary<PlayerEquipment, (Item, GameObject ItemSlot)> playerEquipment = new();

    // �ε� UI 
    public GameObject loadingUI;

    // �ΰ��� ���̺� ������
    
    // �ӽ� �뵵
    public string characterSkin = Defines.body033;
    public string weaponSkin = Defines.weapon005;

    // UI 
    public MainInventory mainInventory;

    // �� ���� ������ ���� �����̳�
    public List<IInGameItem> inGameItems = new ();

    private int BGM_Index = 0;

    // �ΰ��� ���� ���� �� �̵��� �ε� �Ϸ� �� ȣ��
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
        // Todo : �ڵ� ��ü �ʿ� 
        mainInventory = GameObject.FindWithTag("MainInventory")
            .GetComponent<MainInventory>();

        mainInventory.gameObject.SetActive(false);
        loadingUI.SetActive(false);

        InGameItemToMainItem();

        mainInventory.SaveInventory(); 
    }

    // Defines ���� ȣ�� ex) Defines.main 
    public void LoadSceneAsync(string sceneName)
    {
        SoundManager.Instance.ClearSoundPlayerPool();

        loadingUI.SetActive(true);

        // Todo : ���� �� �̸� ����� ���� �ʿ�
        if (sceneName != "InventoryScene")
        {
            mainInventory.SaveMainInventory();
        }


        Addressables.LoadSceneAsync(sceneName).Completed += (op) =>
        {
            //Todo : ���� �� �̸� ����� ���� �ʿ�
            if(sceneName != "InventoryScene")
                loadingUI.SetActive(false);

            SoundManager.Instance.CreateTemporalObjects();
        };

        if (sceneName == "InventoryScene")
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

}
