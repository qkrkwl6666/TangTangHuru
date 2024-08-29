using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public bool isTutorialStage = false;
    private int tutorialProgress = 0;

    public IconLoader iconLoader;
    public List<LevelUpOption> Options_UI;

    private GameObject player;
    private PassiveManager passiveManager;

    private WeaponCreator mainWeapon;
    private List<WeaponCreator> weaponList = new List<WeaponCreator>();
    private List<PassiveData> passiveList = new List<PassiveData>();

    private StringTable stringMgr;

    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            passiveManager = player.GetComponentInChildren<PassiveManager>();
        }
        if (mainWeapon == null)
        {
            mainWeapon = player.GetComponent<PlayerEquipLoader>().GetMainWeapon();
        }

        stringMgr = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);

        SetLevelUpUI();
    }

    private void OnDisable()
    {
        foreach (var option in Options_UI)
        {
            option.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void SetLevelUpUI()
    {
        if (isTutorialStage)
        {
            SetTutorialOptions();
        }
        else
        {
            SetAllOptions();
        }

        SetSelectables();

        if (tutorialProgress > 1)
        {
            isTutorialStage = false;
        }
    }

    private void SetAllOptions()
    {
        weaponList.Clear();
        passiveList.Clear();

        var weaponCount = 0;

        foreach (var weaponCreator in passiveManager.currWeaponCreators) //갖고 있는 액티브 스킬
        {
            weaponCount++; //보유중 숫자
            if (weaponCreator.currLevel < 4)
            {
                weaponList.Add(weaponCreator);
            }
            else if(weaponCreator.currLevel == 4 && weaponCreator.GetEvolovable())
            {
                weaponList.Add(weaponCreator);
            }
        }
        if (weaponCount < 5)
        {
            foreach (var weaponCreator in passiveManager.weaponCreators) //추가될 스킬
            {
                weaponList.Add(weaponCreator);
            }
        }

        foreach (var currPassiveData in passiveManager.currPassiveList) //갖고 있는 패시브
        {
            if (currPassiveData.Level < 7)
            {
                passiveList.Add(currPassiveData);
            }
        }

        foreach (var passiveData in passiveManager.inGamePassiveList) //추가될 패시브
        {
            passiveList.Add(passiveData);
        }
    }

    public void SetSelectables()
    {
        List<int> PassiveOrActive = new List<int>();

        if (weaponList.Count == 0 && passiveList.Count == 0)
        {
            PassiveOrActive.Add(11);
            PassiveOrActive.Add(11);
            PassiveOrActive.Add(11);
        }
        else if (weaponList.Count == 0 && passiveList.Count < 3)
        {
            PassiveOrActive.Add(0);
            PassiveOrActive.Add(11);
            PassiveOrActive.Add(11);
        }
        else if (weaponList.Count == 0)
        {
            PassiveOrActive.Add(0);
            PassiveOrActive.Add(0);
            PassiveOrActive.Add(0);
        }
        else if(weaponList.Count < 3)
        {
            PassiveOrActive.Add(10);
            PassiveOrActive.Add(0);
            PassiveOrActive.Add(0);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (isTutorialStage)
                {
                    if (tutorialProgress == 1)
                    {
                        PassiveOrActive.Add(10);
                    }
                    else
                    {
                        PassiveOrActive.Add(0);
                    }
                }
                else
                {
                    PassiveOrActive.Add(Random.Range(0, 10));
                }
            }
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
            else if(PassiveOrActive[i] < 11)//액티브 선택
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
                        => CreateSkill(selectedCreator));
                }
                else
                {
                    Options_UI[i].GetComponent<Button>().onClick.AddListener(selectedCreator.LevelUpReady);
                }
            }
            else
            {
                Options_UI[i].text_Name.text = "골드";
                Options_UI[i].image_Icon.sprite = iconLoader.SetIconByName("IconChest");
                Options_UI[i].text_Desc.text = "500골드";

                Options_UI[i].GetComponent<Button>().onClick.AddListener(()
                    => InGameInventory.OnCoinAdd?.Invoke(500));
            }
        }

    }

    private void CreateSkill(WeaponCreator creator)
    {
        var skill = Instantiate(creator);
        skill.transform.SetParent(player.transform, false);
        var creators = skill.gameObject.GetComponents<WeaponCreator>();

        foreach (var weaponCreator in creators)
        {
            weaponCreator.SetMainInfo(
                mainWeapon.GetMainDamage(), mainWeapon.GetMainCoolDown(),
                mainWeapon.GetMainCriChance(), mainWeapon.GetMainCriValue(), mainWeapon.GetMainType());
        }
        passiveManager.currWeaponCreators.Add(skill);
        passiveManager.weaponCreators.Remove(creator);
    }


    private void SetTutorialOptions()
    {
        weaponList.Clear();
        passiveList.Clear();

        var weaponCount = 0;

        if (tutorialProgress == 0)
        {
            foreach (var weaponCreator in passiveManager.currWeaponCreators) //갖고 있는 액티브 스킬
            {
                if (!weaponCreator.isMainWeapon && weaponCreator.currLevel < 5)
                {
                    weaponList.Add(weaponCreator);
                }
                weaponCount++; //보유중 숫자
            }
            if (weaponCount < 5)
            {
                foreach (var weaponCreator in passiveManager.weaponCreators) //추가될 스킬
                {
                    weaponList.Add(weaponCreator);
                }
            }
        }
        else if(tutorialProgress == 1)
        {
            foreach (var passiveData in passiveManager.inGamePassiveList) //추가될 패시브
            {
                if (passiveList.Count < 5)
                {
                    passiveList.Add(passiveData);
                }

            }
        }

        tutorialProgress++;

    }


}
