using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public IconLoader iconLoader;
    public List<GameObject> options;
    public PassiveManager passiveManager;

    public List<TextMeshProUGUI> texts_Name; //�̸� �� �ܰ�
    public List<TextMeshProUGUI> texts_Desc; //����
    public List<Image> Icons;

    WeaponCreator mainWeapon;
    List<WeaponCreator> weaponList = new List<WeaponCreator>();
    List<PassiveData> passiveList = new List<PassiveData>();

    public JoystickUI joystickUI;


    private void OnEnable()
    {
        joystickUI.isUI = true;

        var stringMgr = DataTableManager.Instance.Get<StringTable>(DataTableManager.String);

        SetOptions();

        for (int i = 0; i < options.Count; i++)
        {
            var rnd = Random.Range(0, 10);

            if (rnd < 4) //�нú� ����
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
                Icons[i].sprite = iconLoader.SetIconByName("IconPassive");
                texts_Name[i].text = passiveList[select].PassiveName;
                texts_Desc[i].text = passiveList[select].PassiveDesc;
            }
            else //��Ƽ�� ����
            {
                int index = Random.Range(0, weaponList.Count);
                var weaponCreator = passiveManager.weaponCreators[index];

                string selectedName = weaponCreator.weaponDataRef.WeaponName;
                string selectedLevel = (weaponCreator.currLevel + 1).ToString();
                
                Icons[i].sprite = iconLoader.SetIconByName("Icon" + selectedName);
                texts_Name[i].text = stringMgr.Get(selectedName + "_Name_Lv" + selectedLevel).Text;
                texts_Desc[i].text = stringMgr.Get(selectedName + "_Desc_Lv" + selectedLevel).Text;

                if (weaponCreator.currLevel == 0)
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                        => weaponCreator.gameObject.SetActive(true));

                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(weaponCreator.LevelUpReady);
                }
            }
        }


    }

    private void OnDisable()
    {
        joystickUI.isUI = false;

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

        foreach (var weaponCreator in passiveManager.weaponCreators) //���� ��ų �� ���׷��̵� ������ ��
        {
            if (weaponCreator.currLevel != 0)
            {
                weaponCount++; //���� ��ų ����
                if (weaponCreator.currLevel < 5)
                {
                    weaponList.Add(weaponCreator);
                }
            }
        }

        if (weaponCount < 5)
        {
            foreach (var weaponCreator in passiveManager.weaponCreators) //�߰��� ��ų
            {
                if (weaponCreator.currLevel == 0)
                {
                    weaponList.Add(weaponCreator);
                }
            }
        }

        foreach (var currPassiveData in passiveManager.currPassiveList) //���� �ִ� �нú�
        {
            if (currPassiveData.Level < 7)
            {
                passiveList.Add(currPassiveData);
            }
        }

        if (passiveList.Count < 5)
        {
            foreach (var passiveData in passiveManager.passiveDataList) //�߰��� �нú�
            {
                passiveList.Add(passiveData);
            }
        }
    }

}
