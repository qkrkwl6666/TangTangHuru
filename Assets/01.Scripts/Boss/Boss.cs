using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public List<IBossSkill> bossSkills = new List<IBossSkill>();

    public void Update()
    {
        foreach(var skill in bossSkills)
        {
            skill?.Update();
        }
    }
}
