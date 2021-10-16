using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D myRigidbody;
    private Vector2 direction;
	// Use this for initialization
	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        myRigidbody.velocity = direction * speed;
    }
    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Environment")     //Destroy the Missile if it hits a wall or other objects
        {
            Destroy(gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
