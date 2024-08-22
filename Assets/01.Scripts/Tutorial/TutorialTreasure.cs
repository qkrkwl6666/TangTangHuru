using UnityEngine;

public class TutorialTreasure : MonoBehaviour
{
    public InGameTutorialManager inGameTutorialManager;

    private bool isFrist = true;

    private void OnDisable()
    {
        if (isFrist)
            inGameTutorialManager.StartCoroutine(inGameTutorialManager.Tutorial4Start());

        isFrist = false;
    }
}
