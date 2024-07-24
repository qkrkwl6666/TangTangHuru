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

        foreach (var weaponCreator in passiveManager.weaponCreators) //���� �ִ� ��ų
        {
            if(weaponCreator.currLevel != 0 && weaponCreator.currLevel < 5)
            {
                weaponList.Add(weaponCreator);
            }
        }
        if (weaponList.Count < 5) 
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

        if(passiveList.Count < 5)
        {
            foreach (var passiveData in passiveManager.passiveDataList) //�߰��� �нú�
            {
                passiveList.Add(passiveData);
            }
        }


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
                    Debug.Log("�нú� �߰� ������");
                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                         => passiveManager.PassiveLevelUp(passiveList[select]));
                    Debug.Log("�нú� ��ȭ ������");
                }
                texts_Name[i].text = passiveList[select].PassiveName;
                texts_Desc[i].text = "�нú� ����(���̺� ���Ό��)";
                texts_Level[i].text = passiveList[select].Level.ToString();
            }
            else //��Ƽ�� ����
            {
                var select = Random.Range(0, weaponList.Count);
                var weaponCreator = passiveManager.weaponCreators[select];

                if (weaponCreator.currLevel == 0)
                {
                    options[i].GetComponent<Button>().onClick.AddListener(()
                        => weaponCreator.gameObject.SetActive(true));
                    Debug.Log("��Ƽ�� �߰� ������");
                }
                else
                {
                    options[i].GetComponent<Button>().onClick.AddListener(weaponCreator.LevelUpReady);
                    Debug.Log("��Ƽ�� ��ȭ ������");
                }
                texts_Name[i].text = weaponCreator.weaponDataRef.WeaponName;
                texts_Desc[i].text = "��Ƽ�� ����(���̺� ���Ό��)";
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
