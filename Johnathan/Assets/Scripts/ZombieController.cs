using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    public float moveSpeed;

    bool isGrounded = false;
    bool isDead = false;
    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D bc2d;
    GameObject childGo;

	void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
	}
	
	void Update ()
    {
        HasBeenHit();
        MoveZombie();
        
	}

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Player" && isDead == false)
        {
            Destroy(otherObject.gameObject);
        }
        else if(otherObject.gameObject.tag == "Bullet")
        {
            isDead = true;
            childGo = transform.Find("boxCollider").gameObject;

            StopWalk();
            rb.isKinematic = true;
            Destroy(bc2d);
            Destroy(childGo);
            Destroy(gameObject, 2);
        }
        else if(otherObject.gameObject.tag == "GROUND")
        {
            isGrounded = true;
        }
    }

    void HasBeenHit()
    {
        if(isDead == true)
        {
            animator.SetInteger("HasBeenHit", 1);
        }
    }

    void MoveZombie()
    {
        if(isGrounded == true)
        { 
            rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0);
            animator.SetInteger("State", 1);
        }
    }

    public void StopWalk()
    {
        animator.SetInteger("State", 1);
        moveSpeed = 0;
    }
}
