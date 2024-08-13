using UnityEngine;

public class ReRollSkill : MonoBehaviour
{
    private int reRollCountMax = 2;
    public int ReRollCountMax { get { return reRollCountMax; } }

    public bool isEvolved = false;

    private void Start()
    {
        if (isEvolved)
        {
            reRollCountMax = 4;
        }
        else
        {
            reRollCountMax = 2;
        }
    }
}
