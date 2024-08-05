using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MainUI : MonoBehaviour
{
    public List<GameObject> uiGameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
    }

    #region 메인 스테이지 UI

    public TextMeshProUGUI mainStageText;

    public void StageSelectSetActiveTrue()
    {
        uiGameObjects[(int)UIObject.Stage].SetActive(false);
        uiGameObjects[(int)UIObject.StageSelect].SetActive(true);

        // DOTween 함수를 차례대로 수행하게 해줍니다.
        var seq = DOTween.Sequence();

        // DOScale 의 첫 번째 파라미터는 목표 Scale 값, 두 번째는 시간입니다.
        seq.Append(uiGameObjects[(int)UIObject.StageSelect].transform.DOScale(1.1f, 0.2f));
        seq.Append(uiGameObjects[(int)UIObject.StageSelect].transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    #endregion

    #region 스테이지 선택 UI
    public void StageSelectBack()
    {
        uiGameObjects[(int)UIObject.StageSelect].SetActive(false);

        uiGameObjects[(int)UIObject.Stage].SetActive(true);
    }
    public void StageSelectButton()
    {
        uiGameObjects[(int)UIObject.StageSelect].SetActive(false);

        mainStageText.text = DataTableManager.Instance.Get<StageTable>
            (DataTableManager.stage).GetData(GameManager.Instance.CurrentStage + 1).Title;

        uiGameObjects[(int)UIObject.Stage].SetActive(true);
    }

    #endregion

    public void SaveButton()
    {
        SaveDataV1 saveDataV1 = new SaveDataV1();
        //saveDataV1.allItem
    }
}

public enum UIObject
{
    Stage = 0,
    StageSelect = 1,
    Inventory = 2,
}
