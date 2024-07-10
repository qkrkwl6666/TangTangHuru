using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBossSkill
{
    public float Duration { get; }
    public bool IsActive { get; } 
    public float ElapsedTime { get; }

    public void Activate();
    public void DeActivate();
    public void SkillUpdate(float deltaTime);
}
