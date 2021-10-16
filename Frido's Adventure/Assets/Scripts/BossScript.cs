using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{

    public Transform[] spots;
    public float speed;
    public GameObject projectile;
    GameObject player;
    public Transform[] holes;
    Vector3 playerPos;
    public bool vulnerable;
    public float hp = 90;
    bool dead = false;
    bool BossStart = false;
    [SerializeField]
    private List<string> damageSources;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.transform.position.x);

        if (!BossStart && player.transform.position.x > 39)
        {
            StartCoroutine("Boss");
        }

        if (hp <= 0 && !dead)
        {
            dead = true;
            GetComponent<SpriteRenderer>().color = Color.gray;
            StopCoroutine("Boss");
        }
    }

    IEnumerator Boss()
    {
        while (true)
        {
            BossStart = true;
            // FIRST ATTACK

            // move to spot 0
            AudioManager.instance.Play("Alienmove");

            while (transform.position.x != spots[0].position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(spots[0].position.x, transform.position.y), speed);
                yield return null;
            }

            // turn around
            transform.localScale = new Vector2(-1, 1);

            yield return new WaitForSeconds(1f);

            int i = 0;

            // shoot projectiles
            while (i < 9)
            {
                GameObject bullet = (GameObject)Instantiate(projectile, holes[Random.Range(0, 2)].position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * 5;
                AudioManager.instance.Play("Shootendboss");
                i++;
                yield return new WaitForSeconds(.7f);
            }

            // SECOND ATTACK

            GetComponent<Rigidbody2D>().isKinematic = true;

            AudioManager.instance.Play("Alienmove");

            // move to spot 2
            while (transform.position != spots[2].position)
            {
                transform.position = Vector2.MoveTowards(transform.position, spots[2].position, speed);

                yield return null;
            }

            playerPos = player.transform.position;

            yield return new WaitForSeconds(1f);
            GetComponent<Rigidbody2D>().isKinematic = false;


            // ram player
            while (transform.position.x != playerPos.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, transform.position.y), speed + 0.3f);

                yield return null;
            }
            yield return new WaitForSeconds(1f);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            // make vulnerable
            vulnerable = true;
            yield return new WaitForSeconds(4f);
            vulnerable = false;

            // THIRD ATTACK
            Transform temp;

            if (transform.position.x > player.transform.position.x)
            {
                AudioManager.instance.Play("Alienmove");
                temp = spots[1];
            }
            else
            {
                temp = spots[0];
            }

            while (transform.position.x != temp.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(temp.position.x, transform.position.y), speed);

                yield return null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageSources.Contains(collision.tag) && vulnerable)
        {
            TakeDamage();
            vulnerable = false;
        }
    }
    public void TakeDamage()
    {
        hp -= 30;
    }
}
