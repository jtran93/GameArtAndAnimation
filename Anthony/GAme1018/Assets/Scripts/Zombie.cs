using UnityEngine;
using System.Collections;

public class zombie : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

	void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
	    
	}

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if(otherObject.gameObject.tag == "Bullet")
        {
            animator.SetBool("HasBeenHit", true);
        }
    }


}
