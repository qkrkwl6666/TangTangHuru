using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AchieveEntryUI : MonoBehaviour
{
    public TextMeshProUGUI indexUI;
    public TextMeshProUGUI descUI;
    public Image rewardImage;
    public Button rewardButton;


    public void SetDescription(int index)
    {
        indexUI.text = (index + 1).ToString();
        descUI.text = AchievementManager.Instance.achievements[index].description;
        SetRewardState(index);
    }

    private void SetRewardState(int index)
    {
        var currAchieve = AchievementManager.Instance.achievements[index];

        switch (currAchieve.achieveState)
        {
            case AchieveState.Incompleted:
                rewardButton.interactable = false;
                //효과 추가 (이미지 변경등)

                break;
            case AchieveState.Completed:
                rewardButton.interactable = true;

                break;
            case AchieveState.Rewarded:
                rewardButton.interactable = false;

                break;
        }
    }
}
