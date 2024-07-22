using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public List<GameObject> levelUpButtons;
    private List<int> popped;

    private void OnEnable()
    {
        foreach (var buttonObj in levelUpButtons)
        {
            buttonObj.SetActive(false);
        }

        List<int> indices = new List<int>();
        for (int i = 0; i < levelUpButtons.Count; ++i)
        {
            indices.Add(i);
        }

        // Fisher-Yates shuffle �˰��� ����
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // ���� 3�� ��� ����
        for (int i = 0; i < 3; ++i)
        {
            int num = indices[i];
            levelUpButtons[num].SetActive(true);
        }
    }

}
