using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSetUI : MonoBehaviour
{
    public OrbCrafter orbCrafter;
    public ArmorSetEntryUI entryPrefab;
    public GameObject content;

    private List<ArmorSetEntryUI> entries = new();
    private bool sorted = false;
    private void OnEnable()
    {
        if (!sorted)
        {
            for (int i = 1; i <= 7; ++i)
            {
                var entry = Instantiate(entryPrefab, content.transform);
                entry.SetImages(i);
                entry.rewardButton.onClick.AddListener(IncreaseCraftPersent);
                entries.Add(entry);
            }
        }

        RefreshList();

        sorted = true;
    }

    void Start()
    {
        
    }

    private void RefreshList()
    {
        for (int i = 0; i < entries.Count; ++i)
        {
            entries[i].CheckProgress(i + 1);

        }
    }

    private void IncreaseCraftPersent()
    {
        orbCrafter.CreatePersentIncrease(2);
    }
}
