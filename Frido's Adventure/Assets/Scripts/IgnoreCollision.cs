using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {
    [SerializeField]
    private Collider2D[] others;
    private void Awake()
    {
        foreach(Collider2D other in others)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
        }
         
    }
}
