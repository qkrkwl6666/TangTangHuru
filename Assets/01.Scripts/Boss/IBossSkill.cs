using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBossSkill
{
    public int SkillCount { get; set; }
    public bool IsChange { get; set;  } 
    public float Cooldown { get; set;  }

    public void Activate();
    public void DeActivate();
    public void SkillUpdate(float deltaTime);
}
