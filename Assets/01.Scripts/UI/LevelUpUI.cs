using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public List<GameObject> options;
    public PassiveManager passiveManager;

    public List<TextMeshProUGUI> texts_Name; //이름 및 단계
    public List<TextMeshProUGUI> texts_Desc; //설명
    public List<Image> Icons;

    WeaponCreator mainWeapon;
    List<WeaponCreator> weaponList = new List<WeaponCreator>();
    List<PassiveData> passiveList = new List<PassiveData>();

    private void OnEnable()
    {
        var stringMgr = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);

        SetOptions();

        for (int i = 0; i < options.Count; i++)
        {
            var rnd = Random.Range(0, 10);

            if (rnd < 4) //패시브 선택
            {
                var select = Random.Range(0, passiveList.Count);

                if (passiveList[select].Level == 0)
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                        => passiveManager.PassiveAdd(passiveList[select]));
                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                         => passiveManager.PassiveLevelUp(passiveList[select]));
                }
                texts_Name[i].text = passiveList[select].PassiveName;
                texts_Desc[i].text = "패시브 설명";
            }
            else //액티브 선택
            {
                int index = Random.Range(0, weaponList.Count);
                var weaponCreator = passiveManager.weaponCreators[index];

                if (weaponCreator.currLevel == 0)
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                        => weaponCreator.gameObject.SetActive(true));

                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(weaponCreator.LevelUpReady);
                }
                texts_Name[i].text = weaponCreator.weaponDataRef.WeaponName;
                texts_Desc[i].text = "액티브 설명";

            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject option in options)
        {
            option.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void SetOptions()
    {
        weaponList.Clear();
        passiveList.Clear();

        var weaponCount = 0;

        foreach (var weaponCreator in passiveManager.weaponCreators) //가진 스킬 중 업그레이드 가능한 것
        {
            if (weaponCreator.currLevel != 0)
            {
                weaponCount++; //가진 스킬 숫자
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
            foreach (var passiveData in passiveManager.passiveDataList) //추가될 패시브
            {
                passiveList.Add(passiveData);
            }
        }
    }

}
