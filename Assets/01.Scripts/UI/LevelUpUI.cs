using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WeaponData;

public class LevelUpUI : MonoBehaviour
{
    public List<GameObject> options;
    public PassiveManager passiveManager;

    public List<TextMeshProUGUI> texts_Name;
    public List<TextMeshProUGUI> texts_Desc;
    public List<TextMeshProUGUI> texts_Level;

    private void OnEnable()
    {
        List<WeaponCreator> weaponList = new List<WeaponCreator>();
        List<PassiveData> passiveList = new List<PassiveData>();

        foreach (var weaponCreator in passiveManager.weaponCreators) //갖고 있는 스킬
        {
            if(weaponCreator.currLevel != 0 && weaponCreator.currLevel < 5)
            {
                weaponList.Add(weaponCreator);
            }
        }
        if (weaponList.Count < 5) 
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

        if(passiveList.Count < 5)
        {
            foreach (var passiveData in passiveManager.passiveDataList) //추가될 패시브
            {
                passiveList.Add(passiveData);
            }
        }


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
                    Debug.Log("패시브 추가 선택지");
                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                         => passiveManager.PassiveLevelUp(passiveList[select]));
                    Debug.Log("패시브 강화 선택지");
                }
                texts_Name[i].text = passiveList[select].PassiveName;
                texts_Desc[i].text = "패시브 설명(테이블 연결예정)";
                texts_Level[i].text = passiveList[select].Level.ToString();
            }
            else //액티브 선택
            {
                var select = Random.Range(0, weaponList.Count);
                var weaponCreator = passiveManager.weaponCreators[select];

                if (weaponCreator.currLevel == 0)
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                        => weaponCreator.gameObject.SetActive(true));
                    Debug.Log("액티브 추가 선택지");
                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(weaponCreator.LevelUpReady);
                    Debug.Log("액티브 강화 선택지");
                }
                texts_Name[i].text = weaponCreator.weaponDataRef.WeaponName;
                texts_Desc[i].text = "액티브 설명(테이블 연결예정)";
                texts_Level[i].text = weaponCreator.weaponDataRef.Level.ToString();

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

}
