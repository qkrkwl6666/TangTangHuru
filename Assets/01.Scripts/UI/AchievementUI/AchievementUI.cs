using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public MainInventory inventory;
    public AchieveEntryUI entryPrefab;
    public GameObject content;

    private List<AchieveEntryUI> entryList = new();
    private int entryCount;

    private bool sorted = false;

    public List<Image> petScrolls = new();
    public List<Button> petRewardButtons = new();

    private List<int> petIdList = new List<int> { 710001, 710002, 710003, 710004 };

    private void OnEnable()
    {
        if (sorted)
        {
            for (int i = 0; i < entryCount; i++)
            {
                entryList[i].SetDescription(i);
            }
            return;
        }

        entryCount = AchievementManager.Instance.achievements.Count;

        for (int i = 0; i < entryCount; i++)
        {
            entryList.Add(entryPrefab);
        }
        sorted = true;

        for (int i = 0; i < entryCount; i++)
        {
            var index = i;
            entryList[i].SetDescription(i);
            Instantiate(entryList[i].gameObject, content.transform);

            entryList[i].rewardButton.onClick.AddListener(() => GiveRewardScroll(index));
        }

        for (int i = 0; i < petRewardButtons.Count; i++)
        {
            var index = i;
            petRewardButtons[i].onClick.AddListener(() => GiveRewardPet(index));
        }

        CheckScrolls();
        LoadRewardState();
    }

    private void GiveRewardScroll(int index)
    {
        AchievementManager.Instance.achievements[index].achieveState = AchieveState.Rewarded;
        petScrolls[index].gameObject.SetActive(true);

        SaveManager.SaveDataV1.scrollStates.Clear();
        for (int i = 0; i <= petScrolls.Count; i++)
        {
            SaveManager.SaveDataV1.scrollStates.Add(petScrolls[index].gameObject.activeSelf);
        }

        //세개 모았는지 체크
        CheckScrolls();

        SaveManager.SaveDataV1.petRewardStates.Clear();
        for (int i = 0; i <= petRewardButtons.Count; i++)
        {
            SaveManager.SaveDataV1.petRewardStates.Add(petRewardButtons[i].interactable);
        }
    }

    private void CheckScrolls()
    {
        for (int i = 0; i < petRewardButtons.Count; i++)
        {
            if (petScrolls[i * 3].gameObject.activeSelf &&
                petScrolls[i * 3 + 1].gameObject.activeSelf &&
                petScrolls[i * 3 + 2].gameObject.activeSelf)
            {
                petRewardButtons[i].interactable = true;
            }
        }
    }

    private void GiveRewardPet(int num)
    {
        inventory.MainInventoryAddItem(petIdList[num].ToString());
        petRewardButtons[num].interactable = false;

        SaveManager.SaveDataV1.petRewardStates.Clear();
        for (int i = 0; i <= petRewardButtons.Count; i++)
        {
            SaveManager.SaveDataV1.petRewardStates.Add(petRewardButtons[i].interactable);
        }
    }


    private void LoadRewardState()
    {
        if (SaveManager.SaveDataV1.scrollStates.Count == 0)
        {
            foreach (var scroll in petScrolls)
            {
                scroll.gameObject.SetActive(false);
            }
            foreach (var button in petRewardButtons)
            {
                button.interactable = false;
            }

        }
        else
        {
            for (int i = 0; i < petScrolls.Count; i++)
            {
                petScrolls[i].gameObject.SetActive(SaveManager.SaveDataV1.scrollStates[i]);
            }

            for (int i = 0; i < petRewardButtons.Count; i++)
            {
                petRewardButtons[i].interactable = SaveManager.SaveDataV1.petRewardStates[i];
            }
        }
    }
}
