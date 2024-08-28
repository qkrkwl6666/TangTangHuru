using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    private string panelClickSound = "panel";

    public List<GameObject> uiGameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        mainInventory.OnMainInventorySaveLoaded += SaveLoadMainStageText;
    }

    public GameObject allCloseUI;

    #region ���� �������� UI

    public TextMeshProUGUI mainStageText;
    public Image mainStageImage;

    public void SaveLoadMainStageText()
    {
        var data = DataTableManager.Instance.Get<StageTable>(DataTableManager.stage)
            .GetData(GameManager.Instance.CurrentStage);

        mainStageText.text = data.Title;

        if (data.Texture != "-1")
        {
            Addressables.LoadAssetAsync<Sprite>(data.Texture).Completed += (sprite) =>
            {
                mainStageImage.sprite = sprite.Result;
            };

        }
    }


    public void MainStageUIButton()
    {
        for (int i = 0; i < uiGameObjects.Count; i++)
        {
            uiGameObjects[i].SetActive(UIObject.Stage == (UIObject)i);
        }

        SoundManager.Instance.PlaySound2D(panelClickSound);
    }

    public void StageSelectSetActiveTrue()
    {
        uiGameObjects[(int)UIObject.Stage].SetActive(false);
        uiGameObjects[(int)UIObject.StageSelect].SetActive(true);

        // DOTween �Լ��� ���ʴ�� �����ϰ� ���ݴϴ�.
        var seq = DOTween.Sequence();

        // DOScale �� ù ��° �Ķ���ʹ� ��ǥ Scale ��, �� ��°�� �ð��Դϴ�.
        seq.Append(uiGameObjects[(int)UIObject.StageSelect].transform.DOScale(1.1f, 0.2f));
        seq.Append(uiGameObjects[(int)UIObject.StageSelect].transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    #endregion

    #region �������� ���� UI
    public void StageSelectBack()
    {
        uiGameObjects[(int)UIObject.StageSelect].SetActive(false);

        uiGameObjects[(int)UIObject.Stage].SetActive(true);
    }
    public void StageSelectButton()
    {
        uiGameObjects[(int)UIObject.StageSelect].SetActive(false);

        mainStageText.text = DataTableManager.Instance.Get<StageTable>
            (DataTableManager.stage).GetData(GameManager.Instance.CurrentStage).Title;

        uiGameObjects[(int)UIObject.Stage].SetActive(true);
    }

    #endregion

    #region �κ��丮 UI

    public MainInventory mainInventory;

    public void InventoryUIButton()
    {
        mainInventory.RefreshItemSlotUI();
        mainInventory.RefreshPlayerStatusText();

        for (int i = 0; i < uiGameObjects.Count; i++)
        {
            uiGameObjects[i].SetActive(UIObject.Inventory == (UIObject)i);
        }

        SoundManager.Instance.PlaySound2D(panelClickSound);
    }

    #endregion

    public void SaveButton()
    {
        SaveDataV1 saveDataV1 = new SaveDataV1();
        //saveDataV1.allItem
    }

    public void GameStart()
    {
        GameManager.Instance.StartGame();
    }

    public void OpenUI(int uiPanel)
    {
        for (int i = 0; i < uiGameObjects.Count; i++)
        {
            uiGameObjects[i].SetActive((UIObject)uiPanel == (UIObject)i);
        }

        SoundManager.Instance.PlaySound2D(panelClickSound);
    }

    #region UI �˾�

    public EquipPopUp EquipPopUp;

    public void SetActiveEquipPopUpUI(bool active)
    {
        if (active)
        {
            EquipPopUp.gameObject.SetActive(active);
            var seq = DOTween.Sequence();

            seq.Append(EquipPopUp.transform.DOScale(1.1f, 0.2f));
            seq.Append(EquipPopUp.transform.DOScale(1f, 0.1f));

            seq.Play();
        }
        else
        {
            var seq = DOTween.Sequence();

            seq.Append(EquipPopUp.transform.DOScale(0.0f, 0.1f));

            seq.onComplete += () =>
            {
                EquipPopUp.gameObject.SetActive(active);
            };

            seq.Play();
        }
    }

    public void SetEquipPopData(Item item)
    {
        EquipPopUp.SetItemUI(item);
    }

    public void SetUnequipPopData(Item item)
    {
        EquipPopUp.SetItemUI(item, false);
    }

    // �Ҹ�ǰ �˾�
    public ConsumablePopUp consumablePopUp;

    public void SetActiveConsumablePopUpUI(bool active)
    {
        if (active)
        {
            consumablePopUp.gameObject.SetActive(active);
            var seq = DOTween.Sequence();

            seq.Append(consumablePopUp.transform.DOScale(1.1f, 0.2f));
            seq.Append(consumablePopUp.transform.DOScale(1f, 0.1f));

            seq.Play();
        }
        else
        {
            var seq = DOTween.Sequence();

            seq.Append(consumablePopUp.transform.DOScale(0.0f, 0.1f));

            seq.onComplete += () =>
            {
                consumablePopUp.gameObject.SetActive(active);
            };

            seq.Play();
        }
    }

    public void SetConsumablePopUpData(Item item)
    {
        consumablePopUp.SetItemUI(item);
    }

    #endregion

    #region ��� ���� 

    public EquipmentAppraisal equipmentAppraisal;

    public void EquipmentAppraisalUIButton()
    {
        equipmentAppraisal.RefreshGemStoneSlotUI();

        for (int i = 0; i < uiGameObjects.Count; i++)
        {
            uiGameObjects[i].SetActive(UIObject.EquipmentAppraisal == (UIObject)i);
        }

        SoundManager.Instance.PlaySound2D(panelClickSound);
    }

    #endregion

    #region ��UI �˾�

    public PetPopUp petPopUp;

    public void SetActivePetPopUpUI(bool active)
    {
        if (active)
        {
            petPopUp.gameObject.SetActive(active);
            var seq = DOTween.Sequence();

            seq.Append(petPopUp.transform.DOScale(1.1f, 0.2f));
            seq.Append(petPopUp.transform.DOScale(1f, 0.1f));

            seq.Play();
        }
        else
        {
            var seq = DOTween.Sequence();

            seq.Append(petPopUp.transform.DOScale(0.0f, 0.1f));

            seq.onComplete += () =>
            {
                petPopUp.gameObject.SetActive(active);
            };

            seq.Play();
        }
    }

    public void SetEquipPetData(Item item)
    {
        petPopUp.SetItemUI(item);
    }

    public void SetUnequipPetData(Item item)
    {
        petPopUp.SetItemUI(item, false);
    }

    #endregion
}

public enum UIObject
{
    Stage = 0,
    StageSelect = 1,
    Inventory = 2,
    EquipmentAppraisal = 3,
    Carft = 4,
    Shop = 5,
}
