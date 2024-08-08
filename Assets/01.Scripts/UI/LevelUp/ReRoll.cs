using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReRoll : MonoBehaviour
{
    int countMax;
    int currCount = 0;

    void Start()
    {
        GameObject pet = GameObject.FindGameObjectWithTag("Pet");
        if (pet == null)
        {
            gameObject.SetActive(false);
            return;
        }

        var reSkill = pet.GetComponent<ReRollSkill>();
        if(reSkill == null)
        {
            gameObject.SetActive(false);
            return;
        }
        countMax = reSkill.ReRollCountMax;

        GetComponent<Button>().onClick.AddListener(AddReRollCount);
    }

    private void AddReRollCount()
    {
        currCount++;
        if (currCount >= countMax)
        {
            gameObject.SetActive (false);
        }
    }


}
