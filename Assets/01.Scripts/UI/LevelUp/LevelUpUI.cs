using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public IconLoader iconLoader;
    public PassiveManager passiveManager;

    public List<LevelUpOption> Options_UI;

    WeaponCreator mainWeapon;
    List<WeaponCreator> weaponList = new List<WeaponCreator>();
    List<PassiveData> passiveList = new List<PassiveData>();

    StringTable stringMgr;

    private void OnEnable()
    {
        stringMgr = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);
        SetAllOptions();
        SetSelectables();
    }

    private void OnDisable()
    {
        foreach (var option in Options_UI)
        {
            option.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void SetAllOptions()
    {
        weaponList.Clear();
        passiveList.Clear();

        var weaponCount = 0;

        foreach (var weaponCreator in passiveManager.weaponCreators) //선택 가능한 모든 액티브 목록
        {
            if (weaponCreator.currLevel > 0) //보유중
            {
                weaponCount++; //보유중 숫자
                if (weaponCreator.currLevel < 5)
                {
                    weaponList.Add(weaponCreator);
                }
            }
        }

        if (weaponCount < 5)
        {
            foreach (var weaponCreator in passiveManager.weaponCreators) //추가될 스킬
            {
                if (weaponCreator.currLevel == 0)
                {
                    weaponList.Add(weaponCreator);
                }
            }
        }

        foreach (var currPassiveData in passiveManager.currPassiveList) //갖고 있는 패시브
        {
            if (currPassiveData.Level < 7)
            {
                passiveList.Add(currPassiveData);
            }
        }

        if (passiveList.Count < 5)
        {
            foreach (var passiveData in passiveManager.inGamePassiveList) //추가될 패시브
            {
                passiveList.Add(passiveData);
            }
        }
    }

    public void SetSelectables()
    {

        List<int> PassiveOrActive = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            PassiveOrActive.Add(Random.Range(0, 10));
        }

        List<int> passiveNums = Enumerable.Range(0, passiveList.Count).ToList();
        List<int> activeNums = Enumerable.Range(0, weaponList.Count).ToList();
        var random = new System.Random();
        passiveNums = passiveNums.OrderBy(x => random.Next()).ToList();
        activeNums = activeNums.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < Options_UI.Count; i++)
        {

            if (PassiveOrActive[i] < 4) //패시브 선택
            {
                var select = passiveNums[i];
                if (passiveList[select].Level == 0)
                {
                    Options_UI[i].GetComponent<Button>().onClick.AddListener(()
                        => passiveManager.PassiveAdd(passiveList[select]));
                }
                else
                {
                    Options_UI[i].GetComponent<Button>().onClick.AddListener(()
                         => passiveManager.PassiveLevelUp(passiveList[select]));
                }

                Options_UI[i].text_Name.text = passiveList[select].PassiveName + " " + (passiveList[select].Level + 1) + "레벨";
                Options_UI[i].image_Icon.sprite = iconLoader.SetIconByName("IconPassive");
                Options_UI[i].text_Desc.text = passiveList[select].PassiveDesc;
            }
            else //액티브 선택
            {

                var selectedCreator = weaponList[activeNums[i]];

                string selectedName = selectedCreator.weaponDataRef.WeaponName;
                string selectedLevel = (selectedCreator.currLevel + 1).ToString();

                Options_UI[i].text_Name.text = stringMgr.Get(selectedName + "_Name_Lv" + selectedLevel).Text;
                Options_UI[i].image_Icon.sprite = iconLoader.SetIconByName("Icon" + selectedName);
                Options_UI[i].text_Desc.text = stringMgr.Get(selectedName + "_Desc_Lv" + selectedLevel).Text;

                if (selectedCreator.currLevel == 0)
                {
                    Options_UI[i].GetComponent<Button>().onClick.AddListener(()
                        => selectedCreator.gameObject.SetActive(true));
                }
                else
                {
                    Options_UI[i].GetComponent<Button>().onClick.AddListener(selectedCreator.LevelUpReady);
                }
            }
        }

    }

}
