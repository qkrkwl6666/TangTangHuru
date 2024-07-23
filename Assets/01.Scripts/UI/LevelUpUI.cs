using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public List<GameObject> levelUpButtons;
    public PassiveManager passiveManager;

    private void OnEnable()
    {
        foreach (GameObject levelUpButton in levelUpButtons)
        {
            levelUpButton.GetComponent<Button>().onClick.AddListener(passiveManager.PassiveAdd);
        }
    }

}
