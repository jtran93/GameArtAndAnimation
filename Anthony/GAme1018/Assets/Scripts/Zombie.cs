using System.Linq;
using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{


    public float speed;
    private float wallLeft = 0.0f;
    private float wallRight = 5.0f;
    private float patrolWidth = 3.50f;
    Vector3 walkAmount;
    Transform myTrans;
    public Transform target;
    Animator animator;


    //facing
    public GameObject enemyGraphic;
    bool playerInAggroRange;
    bool facingRight;

    Rigidbody2D rb;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wallLeft = transform.position.x - patrolWidth / 2;
        wallRight = transform.position.x + patrolWidth / 2;
        myTrans = this.transform;
        playerInAggroRange = false;
        facingRight = true;
    }

    void FixedUpdate()
    {

        if (playerInAggroRange)
            MoveToPlayer();

        if (!playerInAggroRange)
        {
            walkAmount.x = speed * Time.deltaTime;
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


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInAggroRange = true;
            MoveToPlayer();
        }

    }


    void MoveToPlayer()
    {
        //rotate to look at player
        if(target.transform.position.x > transform.position.x)
        {
            if(facingRight)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            else
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                facingRight = true;

            }
        }
        else if (target.transform.position.x < transform.position.x)
        {
            if (facingRight)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                facingRight = false;
            }
            else
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

            }
        }

    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "Bullet")
        {
            animator.SetBool("HasBeenHit", true);
        }
    }
}