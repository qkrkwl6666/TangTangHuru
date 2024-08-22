using UnityEngine;

public class TutorialLevelUP : MonoBehaviour
{
    public bool LevelUpChat = true;

    public InGameTutorialManager inGameTutorialManager;

    private void OnEnable()
    {
        if(LevelUpChat)
        {
            inGameTutorialManager.StartCoroutine(inGameTutorialManager.Tutorial3Start());
            LevelUpChat = false;
        }

    }

}
