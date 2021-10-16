using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private float patrolTimer;
    private float patrolDuration = 10;
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();
        enemy.Move();
        if (enemy.name == "Wizard" || enemy.name == "Rogue") //Wizard & Rogue Movement
        {
            if (enemy.Target != null && enemy.InShootRange)   
            {
                enemy.ChangeState(new RangedState());
            }
        }
        else //Skeleton Movement
        {
            if (enemy.Target != null && enemy.InMeleeRange)
            {
                enemy.ChangeState(new MeleeState());
            }
            else if (enemy.Target != null && enemy.InShootRange) 
            {
                enemy.ChangeState(new PatrolState());
            }
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.tag == "Edge")             //Turn around if Enemy walks to Edge
        {
            enemy.ChangeDirection();
        }
        if (other.tag == "Missile")         //Destroy Missile when Enemy is hit
        {
            enemy.Target = Player.Instance.gameObject;
        }
    }
    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
