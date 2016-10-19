using System.Linq;

using UnityEngine;

public class Zombie : MonoBehaviour
{


    public float walkSpeed = 2.0f;
    private float wallLeft = 0.0f;
    private float wallRight = 5.0f;
    private float patrolWidth = 2.50f;
    bool facingRight = true;
    Vector3 walkAmount;
    Transform myTrans;
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wallLeft = transform.position.x - patrolWidth / 2;
        wallRight = transform.position.x + patrolWidth / 2;
        myTrans = this.transform;
    }

    void Update()
    {

        walkAmount.x = walkSpeed * Time.deltaTime;
        if (facingRight && transform.position.x >= wallRight)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
            facingRight = false;
        }

        else if (!facingRight && transform.position.x <= wallLeft)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
            facingRight = true;
        }
        transform.Translate(walkAmount);
    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "Bullet")
        {
            animator.SetBool("HasBeenHit", true);
        }
    }
}