using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private List<(IBossSkill skill, float probability)> skills = new List<(IBossSkill, float)>();
    private IBossSkill currentSkill = null;

    private float totalProbability = 0f;
    private float waitDuration = 2f;
    private float waitTime = 0f;

    private void Awake()
    {
        float per100 = 100f;

        var skill = gameObject.AddComponent<BarrageNormal>();
        skills.Add((skill, per100));
        totalProbability = per100;

        SelectSkill();
    }

    public void Update()
    {
        if (currentSkill.IsChange)
        {
            waitTime += Time.deltaTime;
            Debug.Log(waitTime);
            if (waitTime >= waitDuration)
            {
                Debug.Log("SelectSkill");
                SelectSkill();
            }
        }

        currentSkill?.SkillUpdate(Time.deltaTime);
    }

    public void SelectSkill()
    {
        if (currentSkill != null)
        {
            currentSkill.DeActivate();
        }

        waitTime = 0f;
        currentSkill = null;

        float random = Random.Range(0, totalProbability);

        float currentProbability = 0f;

        foreach (var (skill, probability) in skills)
        {
            currentProbability += probability;

            if(random <= currentProbability)
            {
                skill.Activate();
                currentSkill = skill;
                Debug.Log(currentSkill);
            }
        }
    }
}
