using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(2.5f, 4.0f);
        enemy.StopMoving();
    }
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
    public void OnExit(Enemy enemy)
    {

    }
}