using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private float attackTimer;
    private float attackCooldown = 3;
    public bool canAttack = true; 
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();
        if(enemy.InShootRange && !enemy.InMeleeRange && enemy.name != "Skeleton")  //Range-Attack for Rogue and Wizard
        {
            enemy.ChangeState(new RangedState());
        }
        else if(enemy.InShootRange && !enemy.InMeleeRange && enemy.name == "Skeleton")  //Change Skeleton into Patrol-State
        {
            enemy.ChangeState(new PatrolState());
        }
        else if(enemy.Target == null)                   
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }
    public void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)      //check if Enemy is ready to attack again
        {
            canAttack = true;
            attackTimer = 0;
        }
        if (canAttack)
        {
            canAttack = false;
            if(enemy.name == "Skeleton")                        
            {
                AudioManager.instance.Play("Dmg_skeleton");
            }
            else
            {
                AudioManager.instance.Play("Dmg");
            }
            enemy.myAnimator.SetTrigger("attack");  //start Attack
        }
    }
}
