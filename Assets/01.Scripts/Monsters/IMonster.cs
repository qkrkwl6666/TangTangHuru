using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamaged
{
    public void Damaged();
}

public interface IMonster : IDamaged
{
    public void Move();
    public void Attack();
}

public abstract class MeleeMonsters : IMonster
{
    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Damaged()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Move()
    {
        throw new System.NotImplementedException();
    }
}

public class Type1Monster : MeleeMonsters
{
    public override void Move()
    {

    }
}

public class Test : MonoBehaviour
{
    private void Update()
    {
        
    }
}

