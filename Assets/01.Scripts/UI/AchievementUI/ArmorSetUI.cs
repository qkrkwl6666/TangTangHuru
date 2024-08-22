using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSetUI : MonoBehaviour
{
    public ArmorSetEntryUI entryPrefab;
    public GameObject content;

    private List<ArmorSetEntryUI> entries;
    private bool sorted = false;
    private void OnEnable()
    {
        if (sorted)
            return;

        for (int i = 1; i <= 12; ++i)
        {
            var entry = Instantiate(entryPrefab, content.transform);
            entry.SetImages(i);
            entries.Add(entry);
        }

        sorted = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
