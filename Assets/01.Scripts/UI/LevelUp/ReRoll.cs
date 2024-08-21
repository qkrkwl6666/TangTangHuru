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
        if (GameManager.Instance.playerEquipment[PlayerEquipment.Pet].Item1.ItemId != 710002)
        {
            gameObject.SetActive(false);
            return;
        }

        var reSkill = GameObject.FindGameObjectWithTag("Pet").GetComponent<ReRollSkill>();
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
