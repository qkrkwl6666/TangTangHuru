using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    public void Enter();
    public void Update(float deltaTime);
    public void Exit();

}
