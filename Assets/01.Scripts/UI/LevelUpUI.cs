using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static WeaponData;

public class LevelUpUI : MonoBehaviour
{
    public List<GameObject> options;
    public PassiveManager passiveManager;

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



        foreach (GameObject option in options)
        {
            var rnd = Random.Range(0, 10);

            if (rnd < 4) //�нú� ����
            {
                var select = Random.Range(0, passiveList.Count);

                if (passiveList[select].Level == 0)
                {
                    option.GetComponent<Button>().onClick.AddListener(()
                        => passiveManager.PassiveAdd(passiveList[select]));
                    Debug.Log("�нú� �߰� ������");
                }
                else
                {
                    option.GetComponent<Button>().onClick.AddListener(()
                         => passiveManager.PassiveLevelUp(passiveList[select]));
                    Debug.Log("�нú� ��ȭ ������");
                }
            }
            else //��Ƽ�� ����
            {
                var select = Random.Range(0, weaponList.Count);
                var weaponCreator = passiveManager.weaponCreators[select];

                if(weaponCreator.currLevel == 0)
                {
                    option.GetComponent<Button>().onClick.AddListener(() 
                        => weaponCreator.gameObject.SetActive(true));
                    Debug.Log("��Ƽ�� �߰� ������");
                }
                else
                {
                    option.GetComponent<Button>().onClick.AddListener(weaponCreator.LevelUpReady);
                    Debug.Log("��Ƽ�� ��ȭ ������");
                }
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
