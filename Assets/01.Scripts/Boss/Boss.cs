using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public List<IBossSkill> bossSkills = new List<IBossSkill>();

    List<(IBossSkill skill, float probability)> skills = new List<(IBossSkill, float)>();

    private void Awake()
    {
        var skill = gameObject.AddComponent<BarrageNormal>();
        bossSkills.Add(skill);  
    }

    public void Update()
    {
        
        foreach(var skill in bossSkills)
        {
            skill?.SkillUpdate(Time.deltaTime);
        }
    }
}
