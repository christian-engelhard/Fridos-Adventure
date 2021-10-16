using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator myAnimator { get; private set; }
    [SerializeField]
    protected Stat healthStat;
    public abstract bool IsDead { get; }
    [SerializeField]
    protected Transform missilePos;

    [SerializeField]
    protected float movementSpeed;
    protected bool facingRight;
    [SerializeField]
    protected GameObject missilePrefab;
    public bool Attack { get; set; }
    public abstract IEnumerator TakeDamage();
    public bool TakingDamage { get; set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    [SerializeField]
    private EdgeCollider2D swordCollider;
    [SerializeField]
    private List<string> damageSources;
    public abstract void Death();
    // Use this for initialization
    public virtual void Start()
    {
        facingRight = true;
        myAnimator = GetComponent<Animator>();
        healthStat.Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }
    public virtual void Shoot(int value) 
    {
        if (myAnimator.tag == "Player")  //Make the Player look the right way when shooting
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(missilePrefab, missilePos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                tmp.GetComponent<Knife>().Initialize(Vector2.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(missilePrefab, missilePos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                tmp.GetComponent<Knife>().Initialize(Vector2.left);
            }
        }
        else   //Make the Enemy look the right way when shooting
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(missilePrefab, missilePos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                tmp.GetComponent<Missile>().Initialize(Vector2.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(missilePrefab, missilePos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                tmp.GetComponent<Missile>().Initialize(Vector2.left);
            }
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsDead)
        {
            if (damageSources.Contains(other.tag))
            {
                StartCoroutine(TakeDamage());
            }
        }

    }
    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }
}
