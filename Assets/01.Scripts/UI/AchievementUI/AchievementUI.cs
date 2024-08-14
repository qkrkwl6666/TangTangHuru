using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    public AchieveEntryUI entryPrefab;
    public GameObject content;

    private List<AchieveEntryUI> entryList = new();
    private int entryCount;

    private bool sorted = false;

    private void OnEnable()
    {
        if (!sorted)
        {
            entryCount = AchievementManager.Instance.achievements.Count;

            for (int i = 0; i < entryCount; i++)
            {
                entryList.Add(entryPrefab);
            }
            sorted = true;
        }


        for (int i = 0; i < entryCount; i++)
        {
            entryList[i].SetDescription(i);
            Instantiate(entryList[i].gameObject, content.transform);
        }
    }
}
