using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPassiveData : PassiveData
{
    public new int Damage { get; set; } = 0;
    public new float CoolDown { get; set; } = 0f;
    public new float CriticalChance { get; set; } = 0f;
    public new float CriticalValue { get; set; } = 0f;
}
