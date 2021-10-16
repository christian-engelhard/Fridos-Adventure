using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private float shootRange;
    public static int damage;
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }
    public bool InShootRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= shootRange;
            }
            return false;
        }
    }
    private IEnemyState currentState;
    public GameObject Target { get; set; }
    private Canvas healthCanvas;
    public override bool IsDead
    {
        get
        {
            return healthStat.CurrentVal <= 0;
        }
    }
    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
        healthCanvas = transform.GetComponentInChildren<Canvas>();
        damage = 10;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
    }
    private void FixedUpdate()
    {
    }
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }
    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }
    public void Move()
    {
        if (!Attack)
        {
            if (name == "Rogue" || name == "Skeleton")  //Make the Rogue/Skeleton move the right way
            {
                if (GetDirection().x > 0 && transform.position.x < rightEdge.position.x || GetDirection().x < 0 && transform.position.x > leftEdge.position.x)
                {
                    myAnimator.SetFloat("speed", 1);
                    transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
                }
                else if (currentState is PatrolState && name == "Skeleton")
                {
                    ChangeDirection();
                    Target = null;
                }
                else if (currentState is PatrolState && name == "Rogue")
                {
                    ChangeDirection();
                    Target = null;
                }
                else if (currentState is RangedState)
                {
                    Target = null;
                    ChangeState(new IdleState());
                }
            }
            else    //Make the Wizard move the right way
            {
                if (GetDirection().x < 0 && transform.position.x < rightEdge.position.x || GetDirection().x > 0 && transform.position.x > leftEdge.position.x)
                {
                    myAnimator.SetFloat("speed", 1);
                    transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
                }
                else if (currentState is PatrolState)
                {
                    ChangeDirection();
                }
                else if (currentState is RangedState)
                {
                    Target = null;
                    ChangeState(new IdleState());
                }
            }
        }
    }
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }
    public override IEnumerator TakeDamage()
    {
        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }
        healthStat.CurrentVal -= damage;
        Player.maxAttack.gameObject.SetActive(false);
        damage = 10;
        if(IsDead)
        {
            myAnimator.tag = "Dead";
            AudioManager.instance.Play("Die");
            myAnimator.SetTrigger("die");
            yield return null;
        }
    }
    public override void Death()
    {
        Destroy(gameObject);
        healthCanvas.enabled = false;
    }
}
