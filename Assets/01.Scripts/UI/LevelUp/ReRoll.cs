using UnityEngine;
using UnityEngine.UI;

public class ReRoll : MonoBehaviour
{
    int countMax;
    int currCount = 0;

    void Start()
    {
        if (!GameManager.Instance.playerEquipment.ContainsKey(PlayerEquipment.Pet))
        {
            gameObject.SetActive(false);
            return;
        }

        var reSkill = GameObject.FindGameObjectWithTag("Pet").GetComponent<ReRollSkill>();
        if (reSkill != null)
        {
            countMax = reSkill.ReRollCountMax;
        }
        else
        {
            countMax = 0;
        }

        if (countMax == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(AddReRollCount);
        }
    }

    private void AddReRollCount()
    {
        currCount++;
        if (currCount >= countMax)
        {
            gameObject.SetActive(false);
        }
    }


}
