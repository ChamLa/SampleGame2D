using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private IState currentState;

    private bool isRight;

    private Character target;
    public Character Target => target;
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public override void OnInit()
    {
        isRight = true;
        base.OnInit();
        ChangeAnim("idle");
        ChangeState(new IdleState());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }


    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("attack");
    }

    public bool IsTargetInRange()
    {
        return Vector2.Distance(target.transform.position, transform.position) <= attackRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        { 
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool newDirection)
    {
        isRight = newDirection;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    
    internal void SetTarget(Character character)
    {
        this.target = character;

        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else 
        if(Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
}
